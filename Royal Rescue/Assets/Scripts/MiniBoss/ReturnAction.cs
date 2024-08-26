using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ReturnAction : INode
{

    float speed;
    Transform transform;
    Transform target;
    Vector3 dir;
    public float x;
    public ReturnAction(Transform transform, Transform startPos, float speed)
    {
        this.speed = speed;
        this.transform = transform;
        this.target = startPos;

        dir = this.target.position - this.transform.position;
        this.speed = speed;
    }
    public ReturnAction(Transform transform, Transform startPos, float speed, float x)
    {
        this.speed = speed;
        this.transform = transform;
        this.target = startPos;
        this.x = x;

        dir = this.target.position - this.transform.position;
        this.speed = speed;
    }
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        if (Mathf.Abs((transform.position - target.position).x) < 1f)
            return INode.NodeState.Success;

        transform.position += dir.normalized * Time.deltaTime * speed;
        

        return INode.NodeState.Running;
    }
}
