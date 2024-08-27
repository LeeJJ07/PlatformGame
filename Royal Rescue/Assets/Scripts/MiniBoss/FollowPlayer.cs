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
    int groundLayerMask;
    public FollowPlayer(Transform transform, Transform playerTransform, Animator animator, float speed, Hp hp)
    {
        this.hp = hp;
        this.transform = transform;
        this.playerTransform = playerTransform;
        this.animator = animator;
        this.speed = speed;

        this.groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
    }
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        if (hp() <= 0)
            return INode.NodeState.Success;
        if(playerTransform.position.y >= 5f)
        {
            time = 0;
            return INode.NodeState.Success;
        }
        if (time > 2f || !CheckGround())
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

    private bool CheckGround()
    {
        Debug.DrawRay(transform.position + new Vector3(2f, 0f, 0f), Vector3.down, Color.red);
        if (Physics.Raycast(transform.position + new Vector3(2f, 0f, 0f), Vector3.down, 3f, groundLayerMask))
        {
            return true;
        }
        return false;
    }
}
