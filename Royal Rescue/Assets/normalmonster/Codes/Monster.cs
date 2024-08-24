using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Monster : MonoBehaviour
{
    [Header("Monster States")]
    [SerializeField] protected PatrolState patrolState;
    [SerializeField] protected ChaseState chaseState;
    [SerializeField] protected AttackState attackState;
    [SerializeField] protected DeathState deathState;

    protected MonsterStateContext monsterStateContext;
    protected EState curState;

    protected GameObject player;
    protected Animator animator;
    protected Collider coll;

    public bool isDetect;
    public bool isAttack;
    [SerializeField]
    protected float maxHp = 100f;
    protected float curHp = 100f;
    [SerializeField] protected float damage = 10f;

    private float checkObstacleDistance = 0.5f;
    [SerializeField] float toGroundDistance = 1f;
    [SerializeField] float toWallDistance = 0.5f;

    [SerializeField] private float speed = 3f;
    
    [SerializeField] private float detectingDistance = 10f;
    [SerializeField] private float detectingAngle = 50f;

    [SerializeField] private float attackDistance = 1.5f;
    public float facingDir = 1f;
    protected int groundLayerMask;
    protected int wallLayerMask;
    protected int playerMask;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    protected void Start()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!coll) coll = GetComponent<Collider>();

        patrolState = GetComponent<PatrolState>();
        chaseState = GetComponent<ChaseState>();
        attackState = GetComponent<AttackState>();
        deathState = GetComponent<DeathState>();

        monsterStateContext = new MonsterStateContext(this);
        monsterStateContext.Transition(patrolState);
        curState = EState.PATROL;

        isDetect = false;
        isAttack = false;
        maxHp = 100f;
        curHp = maxHp;

        groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
        wallLayerMask = 1 << LayerMask.NameToLayer("Wall");
        playerMask = 1 << LayerMask.NameToLayer("Player");
    }

    protected void Update()
    {
        if (Die())
        {
            UpdateState(EState.DEATH);
            monsterStateContext.CurrentState.UpdateState();
            return;
        }
        switch (curState)
        {
            case EState.PATROL:
                if (CanSeePlayer(0.5f))
                {
                    isDetect = true;
                    UpdateState(EState.CHASE);
                }
                break;
            case EState.CHASE:
                if (CantChase() || !CanSeePlayer(0.5f))
                    UpdateState(EState.PATROL);
                if (CanAttackPlayer())
                    UpdateState(EState.ATTACK);
                break;
            case EState.ATTACK:
                if (!CanAttackPlayer())
                {
                    isDetect = false;
                    UpdateState(EState.CHASE);
                }
                break;
        }
        monsterStateContext.CurrentState.UpdateState();
    }

    protected void UpdateState(EState nextState)
    {
        if (curState == nextState)
            return;
        curState = nextState;

        switch (curState)
        {
            case EState.PATROL:
                monsterStateContext.Transition(patrolState);
                break;
            case EState.CHASE:
                monsterStateContext.Transition(chaseState);
                break;
            case EState.ATTACK:
                monsterStateContext.Transition(attackState);
                break;
            case EState.DEATH:
                monsterStateContext.Transition(deathState);
                break;
        }
    }
    #region 필요한 setter, getter
    public float getSpeed() { return speed; }
    public void setSpeed(float speed) { this.speed = speed; }
    public float getDamage() { return damage; }
    public float getFacingDir() { return facingDir; }
    public float getToGroundDistance() { return toGroundDistance; }
    #endregion

    #region 전이조건
    protected bool CanSeePlayer(float eyeHeight)
    {
        Vector3 myPos = transform.position + Vector3.up * eyeHeight;

        float lookingAngle = transform.eulerAngles.x + (90f - 90f * facingDir);
        Vector3 lookDir = AngleToDir(lookingAngle);

        if (Physics.CheckSphere(myPos, detectingDistance, playerMask))
        {
            Vector3 targetPos = player.transform.position;
            Vector3 targetDir = (targetPos - myPos).normalized;

            if (CheckWall(transform.position, targetDir, (targetPos - myPos).magnitude))
                return false;
            if (CheckGround(transform.position, targetDir, (targetPos - myPos).magnitude / 2.5f))
                return false;

            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
            if (targetAngle <= detectingAngle * 0.5f)
                return true;
        }
        return false;
    }
    protected bool CantChase()
    {
        return getDistancePlayer() > detectingDistance;
    }

    protected bool CanAttackPlayer()
    {
        return getDistancePlayer() < attackDistance;
    }

    protected bool Die()
    {
        return curHp <= 0;
    }
    #endregion

    #region 피격
    private void OnTriggerEnter(Collider other)
    {
        if (!animator.GetBool("isLive"))
            return;

        if (other == null)
            return;

        if (other.gameObject.tag == "PlayerAttack")
            StartCoroutine(OnDamage());

    }
    IEnumerator OnDamage()
    {
        animator.SetTrigger("takeAttack");
        curHp -= player.GetComponent<PlayerController>().damage;

        coll.enabled = false;
        yield return new WaitForSeconds(0.5f);
        coll.enabled = true;
        Debug.Log("공격 받았음");
    }
    #endregion

    #region 지나칠 때
    private void OnTriggerStay(Collider other)
    {
        if (!animator.GetBool("isLive"))
            return;
        if (other == null)
            return;
        if (other.gameObject.tag == "Player")
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);

            Vector3 dir = new Vector3(other.gameObject.transform.position.x - transform.position.x, 0f, 0f).normalized;
            rb.AddForce(dir * 15f, ForceMode.Impulse);
        }
    }
    #endregion

    #region 공용함수(각도->방향, 땅체크, 벽체크, Flip, 다른 오브젝트와의 거리) 캐릭터 컨트롤러
    public Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0);
    }
    public bool CheckGround(Vector3 origin, Vector3 direction)
    {
        if (Physics.Raycast(origin + new Vector3(checkObstacleDistance, 0f, 0f), direction, toGroundDistance, groundLayerMask))
        {
            return true;
        }
        return false;
    }
    public bool CheckGround(Vector3 origin, Vector3 direction, float distance)
    {
        if (Physics.Raycast(origin + new Vector3(checkObstacleDistance, 0f, 0f), direction, distance, groundLayerMask))
        {
            return true;
        }
        return false;
    }
    public bool CheckWall(Vector3 origin, Vector3 direction)
    {
        if (Physics.Raycast(origin, direction, toWallDistance, wallLayerMask))
        {
            return true;
        }
        return false;
    }
    public bool CheckWall(Vector3 origin, Vector3 direction, float distance)
    {
        if (Physics.Raycast(origin, direction, distance, wallLayerMask))
        {
            return true;
        }
        return false;
    }
    public void FlipX()
    {
        transform.rotation = Quaternion.Euler(0f, 180f + facingDir * 60f, 0f);
        checkObstacleDistance *= -1;
        facingDir *= -1;
    }
    public float getDistancePlayer()
    {
        return (player.transform.position - transform.position).magnitude;
    }
    public float getDirectionPlayerX()
    {
        float monsterX = transform.position.x;
        float playerX = player.transform.position.x;
        return (playerX - monsterX) / Mathf.Abs(playerX - monsterX);
    }
    public bool LookPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        if (getFacingDir() > 0 && direction.x < 0)
            return false;
        if (getFacingDir() < 0 && direction.x >= 0)
            return false;
        return true;
    }
    #endregion
}
