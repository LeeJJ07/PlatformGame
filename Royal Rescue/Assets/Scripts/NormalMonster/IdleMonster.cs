using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class IdleMonster : Monster
{
    [Header("Additional State")]
    [SerializeField] private IState idleState;

    [SerializeField] float awakeDistance;

    bool isAwake = false;
    new void Awake()
    {
        base.Awake();
        isAwake = false;
    }
    new void Start()
    {
        base.Start();
        awakeDistance = data.AwakeRange;

        animator.SetBool("isLive", false);
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
    private void OnEnable()
    {
        transform.position = initialPos;

        if (animator && animator.GetBool("isDie"))
            gameObject.SetActive(false);

        if (animator && animator.GetBool("isLive"))
        {
            curState = EState.PATROL;
            monsterStateContext.Transition(patrolState);
        }
    }
    new void Update()
    {
        if (!isAwake && IsPossibleAwake())
        {
            isAwake = true;
            animator.SetBool("isLive", true);
            UpdateState(EState.CHASE);
            return;
        }
        base.Update();
    }

    bool IsPossibleAwake()
    {
        float diffY = Mathf.Abs(player.transform.position.y - transform.position.y);
        return (awakeDistance > getDistancePlayer()) && (diffY < 1f);
    }
}
