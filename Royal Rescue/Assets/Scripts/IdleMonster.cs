using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class IdleMonster : Monster
{
    [Header("Additional State")]
    [SerializeField] private IState idleState;

    [SerializeField] float awakeDistance = 3f;

    bool isAwake = false;

    new void Start()
    {
        base.Start();

        switch (this.tag)
        {
            case "ChestMonster":
                idleState = GetComponent<ChestIdleState>();
                break;
            default:
                idleState = GetComponent<IdleState>();
                break;
        }

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
        return awakeDistance > getDistancePlayer();
    }
}
