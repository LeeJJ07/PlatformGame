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
    [Header("Monster Info")]
    [SerializeField]
    protected NormalMonsterData data;

    [Header("Monster States")]
    [SerializeField] protected PatrolState patrolState;
    [SerializeField] protected ChaseState chaseState;
    [SerializeField] protected AttackState attackState;
    [SerializeField] protected DeathState deathState;

    protected MonsterStateContext monsterStateContext;
    protected EState curState;

    protected PlayerControlManagerFix playerControl;
    protected GameObject player;

    protected Animator animator;
    protected Collider coll;

    public bool isDetect;
    public bool isAttack;
    public bool detecting;

    protected float maxHp;
    protected float curHp;
    protected int damage;
    protected float walkSpeed;
    protected float runSpeed;
    protected float detectingDistance;
    protected float detectingAngle = 50f;
    protected float attackDistance;

    protected Vector3 initialPos;

    private float checkObstacleDistance = 0.5f;
    [SerializeField] float toGroundDistance = 1f;
    [SerializeField] float toWallDistance = 0.5f;
    
    public float facingDir = 1f;
    protected int groundLayerMask;
    protected int wallLayerMask;
    protected int playerMask;

    protected void Awake()
    {
        initialPos = transform.position;
    }

    protected void Start()
    {
        playerControl = GameDirector.instance.PlayerControl;
        player = playerControl.gameObject;
        
        if (!animator) animator = GetComponent<Animator>();
        if (!coll) coll = GetComponent<Collider>();

        patrolState = GetComponent<PatrolState>();
        chaseState = GetComponent<ChaseState>();
        attackState = GetComponent<AttackState>();
        deathState = GetComponent<DeathState>();

        monsterStateContext = new MonsterStateContext(this);
        monsterStateContext.Transition(patrolState);
        curState = EState.PATROL;

        maxHp = data.Hp;
        curHp = maxHp;
        damage = data.Damage;
        walkSpeed = data.MoveSpeed;
        runSpeed = data.RunSpeed;
        detectingDistance = data.SightRange;
        attackDistance = data.AttackRange;

        isDetect = false;
        isAttack = false;

        groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
        wallLayerMask = 1 << LayerMask.NameToLayer("Wall");
        playerMask = 1 << LayerMask.NameToLayer("Player");

    }
    private void OnEnable()
    {
        transform.position = initialPos;
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
    #region �ʿ��� setter, getter
    public float getWalkSpeed() { return walkSpeed; }
    public float getRunSpeed() { return runSpeed; }
    public int getDamage() { return damage; }
    public float getFacingDir() { return facingDir; }
    public float getToGroundDistance() { return toGroundDistance; }
    #endregion

    #region ��������
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

    #region �ǰ�
    private void OnTriggerEnter(Collider other)
    {
        if (!animator.GetBool("isLive"))
            return;

        if (other == null)
            return;


        if (other.gameObject.tag == "Weapon"
            || other.gameObject.tag == "Bomb"
            || other.gameObject.tag == "SlashAttack")
        {
            if(!LookPlayer())
                FlipX();
            StartCoroutine(OnDamage(other.gameObject.tag));
        }
    }
    IEnumerator OnDamage(string tag)
    {
        animator.SetTrigger("takeAttack");
        switch (tag)
        {
            case "Weapon":
                curHp -= 10f; //playerControl.getDamage();
                Debug.Log("기본 공격 받았다.");
                break;
            case "Bomb":
                curHp -= 20f; //playerControl.getDamage();
                Debug.Log("폭탄 공격 받았다.");
                break;
            case "SlashAttack":
                curHp -= 30f; //playerControl.getDamage();
                Debug.Log("슬래쉬 공격 받았다.");
                break;
        }

        coll.enabled = false;
        yield return new WaitForSeconds(0.5f);
        coll.enabled = true;
        Debug.Log("���� �޾���");
    }
    #endregion

    #region ����ĥ ��
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

    #region �����Լ�(����->����, ��üũ, ��üũ, Flip, �ٸ� ������Ʈ���� �Ÿ�) ĳ���� ��Ʈ�ѷ�
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
