using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossSkill2Attack : INode
{
    Transform transform;
    Transform playerTransform;
    Animator animator;
    float time = 0f;
    bool isIdle = false;
    public MiniBossSkill2Attack(Transform transform, Transform playerTransform, Animator animator)
    {
        this.transform = transform;
        this.playerTransform = playerTransform;
        this.animator = animator;
    }
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        float dirX = (playerTransform.position - transform.position).x;
        if(time > 7f)
        {
            transform.GetChild(4).gameObject.SetActive(false);
            time = 0f;
            isIdle = false;
            return INode.NodeState.Success;
        }
        else if (!isIdle && time > 2f)
        {
            transform.GetChild(4).gameObject.SetActive(true);
            animator.SetTrigger("idle");
            isIdle = true;
        }
        if (time == 0)
        {
            transform.GetChild(4).gameObject.transform.localRotation = Quaternion.Euler(0f, -dirX / Mathf.Abs(dirX) * 30f, 0f);
            
            animator.SetTrigger("jump");
        }
        time += Time.deltaTime;
        return INode.NodeState.Running;
    }
}
