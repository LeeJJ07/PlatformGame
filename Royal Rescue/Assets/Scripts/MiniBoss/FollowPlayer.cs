using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : INode
{
    Transform transform;
    Transform playerTransform;

    float speed;
    float time = 0f;
    public FollowPlayer(Transform transform, Transform playerTransform, float speed)
    {
        this.transform = transform;
        this.playerTransform = playerTransform;
        this.speed = speed;
    }
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        if (time > 3f)
        {
            time = 0f;
            return INode.NodeState.Success;
        }else if(time > 2f)
        {
            time += Time.deltaTime;
            return INode.NodeState.Running;
        }
        float lookDir = (transform.position.x - playerTransform.position.x) / Mathf.Abs(transform.position.x - playerTransform.position.x);
        transform.rotation = Quaternion.Euler(0f, 180f + 60f * lookDir, 0f);

        Vector3 nextPos = (playerTransform.position - transform.position).normalized * Time.deltaTime * speed;
        transform.position += new Vector3(nextPos.x, 0f, 0f);

        time += Time.deltaTime;

        return INode.NodeState.Running;
    }
}
