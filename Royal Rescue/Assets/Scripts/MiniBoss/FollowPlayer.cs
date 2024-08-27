using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : INode
{
    public delegate float Hp();
    Hp hp;

    Transform transform;
    Transform playerTransform;
    Animator animator;

    float speed;
    float time = 0f;
    public FollowPlayer(Transform transform, Transform playerTransform, Animator animator, float speed, Hp hp)
    {
        this.hp = hp;
        this.transform = transform;
        this.playerTransform = playerTransform;
        this.animator = animator;
        this.speed = speed;
    }
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        if (hp() <= 0)
            return INode.NodeState.Success;
        if (time > 2f)
        {
            time = 0;
            return INode.NodeState.Success;
        }
        if (time == 0)
            animator.SetTrigger("walk");
        Vector3 nextPos = (playerTransform.position - transform.position);

        float lookDir = (nextPos.x) / Mathf.Abs(nextPos.x);
        transform.rotation = Quaternion.Euler(0f, 180f - 60f * lookDir, 0f);
        
        if(nextPos.magnitude < 5f)
        {
            time = 0;
            return INode.NodeState.Success;
        }
        float nextX = nextPos.normalized.x * Time.deltaTime * speed;
        transform.position += new Vector3(nextX, 0f, 0f);

        time += Time.deltaTime;

        return INode.NodeState.Running;
    }
}
