using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAttackRange : INode
{
    Transform transform;
    Transform playerTransform;
    float range;
    public CheckAttackRange(Transform transform, Transform playerTransform, float range) 
    {
        this.transform = transform;
        this.playerTransform = playerTransform;
        this.range = range;
    }
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        float mag = (transform.position - playerTransform.position).magnitude;

        if (mag < range)
            return INode.NodeState.Success;
        return INode.NodeState.Failure;
    }
}
