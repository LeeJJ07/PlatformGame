using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHp : INode
{
    EnemyAI enemyAI;
    float hpCondition;
    public CheckHp(EnemyAI enemyAI, float hpCondition)
    {
        this.enemyAI = enemyAI;
        this.hpCondition = hpCondition;
    }
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        if(enemyAI.Hp > hpCondition)
            return INode.NodeState.Success;
        else
            return INode.NodeState.Failure;
    }
}
