using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Monster : MonoBehaviour
{
    [Header("Enemy States")]
    [SerializeField] private PatrolState patrolState;
    [SerializeField] private ChaseState chaseState;
    [SerializeField] private AttackState attackState;
    [SerializeField] private DeathState deathState;

    private MonsterStateContext monsterStateContext;
    private EState curState;

    public GameObject player;
    private Animator animator;


    public bool isDetect { get; set; }
    [SerializeField] private float hp = 100f;

    [SerializeField] private float damage = 10f;

    private float checkObstacleDistance = 0.5f;
    private float toGroundDistance = 1f;
    private float toWallDistance = 0.5f;

    private float speed = 3f;
    public float facingDir = 1f;

    private float detectingDistance = 4f;
    private float detectingAngle = 50f;

    private float chaseDistance = 6f;
    private float attackDistance = 1.5f;

    private int groundLayerMask;
    private int wallLayerMask;
    private int playerMastk;

    private void Start()
    {
        if (!animator) animator = GetComponent<Animator>();
        monsterStateContext = new MonsterStateContext(this);
        monsterStateContext.Transition(patrolState);
        curState = EState.PATROL;

        isDetect = false;
        hp = 100f;

        groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
        wallLayerMask = 1 << LayerMask.NameToLayer("Wall");
        playerMastk = 1 << LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        if (Die())
            UpdateState(EState.DEATH);
        else
        {
            switch (curState)
            {
                case EState.PATROL:
                    if (CanSeePlayer())
                    {
                        isDetect = true;
                        UpdateState(EState.CHASE);
                    }
                    break;
                case EState.CHASE:
                    if (CantChase() && !CanSeePlayer())
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
        }
        monsterStateContext.CurrentState.UpdateState();
    }

    private void UpdateState(EState nextState)
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
    public float getSpeed() { return speed; }
    public void setSpeed(float speed) { this.speed = speed; }
    public float getDamage() { return damage; }
    public float getFacingDir() { return facingDir; }
    public void setFacingDir(float dir) { this.facingDir = dir / Mathf.Abs(dir); }
    bool CanSeePlayer()
    {
        //player가 시야각 안에 있는가
        Vector3 myPos = transform.position + Vector3.up * 0.5f;

        float lookingAngle = transform.eulerAngles.x + (90f - 90f * facingDir);  //캐릭터가 바라보는 방향의 각도
        Vector3 lookDir = AngleToDir(lookingAngle);

        if (Physics.CheckSphere(myPos, detectingDistance, playerMastk))
        {
            Vector3 targetPos = player.transform.position;
            Vector3 targetDir = (targetPos - myPos).normalized;

            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
            if (targetAngle <= detectingAngle * 0.5f)
                return true;
        }
        return false;
    }
    bool CantChase()
    {
        return (player.transform.position - transform.position).magnitude > chaseDistance;
    }
    Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0);
    }
    private bool CanAttackPlayer()
    {
        return (player.transform.position - transform.position).magnitude < attackDistance;
    }

    private bool Die()
    {
        return hp <= 0;
    }
    public bool CheckGround(Vector3 origin, Vector3 direction)
    {
        Debug.DrawRay(origin + new Vector3(checkObstacleDistance, 0f, 0f), Vector3.down, Color.red);
        if (Physics.Raycast(origin + new Vector3(checkObstacleDistance, 0f, 0f), direction, toGroundDistance, groundLayerMask))
        {
            return true;
        }
        return false;
    }
    public bool CheckWall(Vector3 origin)
    {
        Debug.DrawRay(origin, new Vector3(facingDir, 0f, 0f), Color.red);
        if (Physics.Raycast(origin, new Vector3(facingDir, 0f, 0f), toWallDistance, wallLayerMask))
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



    //데미지 입는거 임시 구현
    private void takeAttack(float takeAttackDamage)
    {
        hp -= takeAttackDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        float playerDamage = 20f;
        if (other == null)
            return;
        if (other.gameObject.tag == "PlayerAttack")
        {
            takeAttack(playerDamage);
            animator.SetTrigger("takeAttack");

            Vector3 dir = (other.gameObject.transform.position - transform.position).normalized;
            other.gameObject.GetComponent<Rigidbody>().AddForce(dir * 10f, ForceMode.Impulse);
        }
    }
}
