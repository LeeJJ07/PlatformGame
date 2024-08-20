using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChecktoTargetDistance : INode
{
    EnemyAI enemyAI;
    float targetDistance;
    public ChecktoTargetDistance(EnemyAI enemyAI, float targetDistance)
    {
        this.enemyAI = enemyAI;
        this.targetDistance = targetDistance;
    }
    public void AddNode(INode node){ }

    public INode.NodeState Evaluate()
    {
        //float distance = Vector3.Distance(enemyAI.transform.position, enemyAI.Target.position);
        float distance = XDistance(enemyAI.transform.position.x, enemyAI.Target.position.x);
        if (distance >= targetDistance)
        {
            Debug.Log("distance Success");
            return INode.NodeState.Success;
        }

        Debug.Log("distance Fail");
        return INode.NodeState.Failure;
            
    }
    float XDistance(float x1, float x2)
    {
        return Mathf.Abs(x1 - x2);
    }
}
