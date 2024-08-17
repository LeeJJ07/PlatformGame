using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTargetHeight : INode
{
    EnemyAI enemyAI;
    public CheckTargetHeight(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }

    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
       
        return INode.NodeState.Success;
    }
}
