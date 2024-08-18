using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleMonster : Monster
{
    [Header("Additional State")]
    [SerializeField] private IdleState idleState;

    [SerializeField] float awakeDistance = 1.5f;

    bool isAwake = false;

    new void Start()
    {
        base.Start();

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
