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

        float lookDir = (transform.position.x - playerTransform.position.x) / Mathf.Abs(transform.position.x - playerTransform.position.x);
        transform.rotation = Quaternion.Euler(0f, 180f + 60f * lookDir, 0f);

        Vector3 nextPos = (playerTransform.position - transform.position).normalized * Time.deltaTime * speed;
        transform.position += new Vector3(nextPos.x, 0f, 0f);

        time += Time.deltaTime;

        return INode.NodeState.Running;
    }
}
