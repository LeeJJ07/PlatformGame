using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChecktoTargetDistance : INode
{
    INode node;
    Transform transform;
    Transform target;
    float targetDistance;
    public ChecktoTargetDistance(Transform transform, Transform target, float targetDistance)
    {
        this.transform = transform;
        this.target = target;
        this.targetDistance = targetDistance;
    }
    public void AddNode(INode node) {}


    public INode.NodeState Evaluate()
    {
        //float distance = Vector3.Distance(enemyAI.transform.position, enemyAI.Target.position);
        float distance = HorDistance(transform.position.x,target.position.x);
        if (distance >= targetDistance)
            return INode.NodeState.Success;
        
        return INode.NodeState.Failure;
            
    }
    float HorDistance(float x1, float x2)
    {
        return Mathf.Abs(x1 - x2);
    }
}
