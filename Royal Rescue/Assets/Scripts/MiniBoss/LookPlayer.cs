using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPlayer : INode
{
    Transform transform;
    Transform target;
    public LookPlayer(Transform transform, Transform target)
    {
        this.transform = transform;
        this.target = target;
    }
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        float lookDir = (transform.position.x - target.position.x)/ Mathf.Abs(transform.position.x - target.position.x);
        transform.rotation = Quaternion.Euler(0f, 180f + 60f * lookDir, 0f);
        return INode.NodeState.Failure;
    }
}
