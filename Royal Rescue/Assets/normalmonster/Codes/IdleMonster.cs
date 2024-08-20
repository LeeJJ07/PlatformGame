using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class IdleMonster : Monster
{
    [Header("Additional State")]
    [SerializeField] private IdleState idleState;

    [SerializeField] float awakeDistance = 3f;

    bool isAwake = false;

    new void Start()
    {
        base.Start();

        idleState = GetComponent<IdleState>(); 

        monsterStateContext.Transition(idleState);
        curState = EState.IDLE;

    }
    new void Update()
    {
        if (!isAwake && IsPossibleAwake())
        {
            isAwake = true;
            UpdateState(EState.CHASE);
            return;
        }
        base.Update();
    }

    bool IsPossibleAwake()
    {
        return awakeDistance > getDistanceOther(player);
    }
}
