//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AirMonster : Monster
//{


//    new void Start()
//    {
//        if (!animator) animator = GetComponent<Animator>();
//        if (!coll) coll = GetComponent<Collider>();

//        patrolState = GetComponent<AirPatrolState>();
//        chaseState = GetComponent<AirChaseState>();
//        attackState = GetComponent<RangedAttackState>();
//        deathState = GetComponent<DeathState>();

//        monsterStateContext = new MonsterStateContext(this);
//        monsterStateContext.Transition(patrolState);
//        curState = EState.PATROL;

//        isDetect = false;
//        isAttack = false;
//        maxHp = 100f;
//        curHp = maxHp;

//        groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
//        wallLayerMask = 1 << LayerMask.NameToLayer("Wall");
//        playerMastk = 1 << LayerMask.NameToLayer("Player");
//    }

//    new void Update()
//    {
//        if (Die())
//        {
//            UpdateState(EState.DEATH);
//            monsterStateContext.CurrentState.UpdateState();
//            return;
//        }
//        switch (curState)
//        {
//            case EState.PATROL:
//                if (CanSeePlayer())
//                {
//                    isDetect = true;
//                    UpdateState(EState.CHASE);
//                }
//                break;
//            case EState.CHASE:
//                if (CantChase() && !CanSeePlayer())
//                    UpdateState(EState.PATROL);
//                if (CanAttackPlayer())
//                    UpdateState(EState.ATTACK);
//                break;
//            case EState.ATTACK:
//                if (!CanAttackPlayer())
//                {
//                    isDetect = false;
//                    UpdateState(EState.CHASE);
//                }
//                break;
//        }
//        monsterStateContext.CurrentState.UpdateState();
//    }

//    new void UpdateState(EState nextState)
//    {
//        if (curState == nextState)
//            return;
//        curState = nextState;

//        switch (curState)
//        {
//            case EState.PATROL:
//                monsterStateContext.Transition(patrolState);
//                break;
//            case EState.CHASE:
//                monsterStateContext.Transition(chaseState);
//                break;
//            case EState.ATTACK:
//                monsterStateContext.Transition(attackState);
//                break;
//            case EState.DEATH:
//                monsterStateContext.Transition(deathState);
//                break;
//        }
//    }
//    #region 전이조건
//    bool CanSeePlayer()
//    {
//        //player가 시야각 안에 있는가
//        Vector3 myPos = transform.position + Vector3.up * 0.5f;

//        float lookingAngle = transform.eulerAngles.x + (90f - 90f * facingDir);  //캐릭터가 바라보는 방향의 각도
//        Vector3 lookDir = AngleToDir(lookingAngle);

//        if (Physics.CheckSphere(myPos, detectingDistance, playerMastk))
//        {
//            Vector3 targetPos = player.transform.position;
//            Vector3 targetDir = (targetPos - myPos).normalized;

//            if (CheckWall(transform.position, detectingDistance))
//                return false;

//            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
//            if (targetAngle <= detectingAngle * 0.5f)
//                return true;
//        }
//        return false;
//    }
//    bool CantChase()
//    {
//        return getDistancePlayer() > chaseDistance;
//    }

//    private bool CanAttackPlayer()
//    {
//        return getDistancePlayer() < attackDistance;
//    }
//    #endregion
//}
