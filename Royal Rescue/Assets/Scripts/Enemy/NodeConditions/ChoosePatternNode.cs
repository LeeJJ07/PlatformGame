using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosePatternNode : INode
{
    EnemyAI enemyAI;
    int count = 0;
    
    public ChoosePatternNode(EnemyAI enemyAI,int count)
    {
        this.enemyAI = enemyAI;
        this.count = count;
    }
    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        if (!enemyAI)
            return INode.NodeState.Failure;
        
        enemyAI.AttackIndex= Random.Range(0, count);
        Debug.Log("ATTACK INDEX: " + enemyAI.AttackIndex);
        return INode.NodeState.Success;
    }
}
