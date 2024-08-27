using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDelay : INode
{
    Animator animator;
    float duration = 0;
    float span = 0;
    bool isActiveIdle = false;
    public ActionDelay(Animator animator, float duration)
    {
        this.animator = animator;
        this.duration = duration;
    }

    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        if (!isActiveIdle)
        {
            animator.SetTrigger("idle");
            isActiveIdle = true;
        }
        span += Time.deltaTime;
        if (span >= duration)
        {
            span = 0;
            isActiveIdle = false;
            return INode.NodeState.Success;
        }
        return INode.NodeState.Running;
    }
}
