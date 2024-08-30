using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMonster : Monster
{
    new void Start()
    {
        playerControl = GameDirector.instance.PlayerControl;
        player = playerControl.gameObject;

        if (!animator) animator = GetComponent<Animator>();
        if (!coll) coll = GetComponent<Collider>();
        
        patrolState = GetComponent<PatrolState>();
        attackState = GetComponent<RangedAttackState>();
        deathState = GetComponent<DeathState>();

        monsterStateContext = new MonsterStateContext(this);
        monsterStateContext.Transition(patrolState);
        curState = EState.PATROL;

        isDetect = false;

        maxHp = data.Hp;
        curHp = maxHp;
        damage = data.Damage;
        walkSpeed = data.MoveSpeed;
        runSpeed = data.RunSpeed;
        detectingDistance = data.SightRange;
        attackDistance = data.AttackRange;

        groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
        wallLayerMask = 1 << LayerMask.NameToLayer("Wall");
        playerMask = 1 << LayerMask.NameToLayer("Player");
    }

    new void Update()
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
                if (CanSeePlayer(1.4f))
                {
                    isDetect = true;
                    UpdateState(EState.ATTACK);
                }
                break;
            case EState.ATTACK:
                if (!CanSeePlayer(1.4f))
                {
                    isDetect = false;
                    UpdateState(EState.PATROL);
                }
                break;
        }
        monsterStateContext.CurrentState.UpdateState();
    }
}
