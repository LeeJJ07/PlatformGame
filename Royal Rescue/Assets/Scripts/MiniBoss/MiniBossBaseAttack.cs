using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossBaseAttack : INode
{
    Animator animator;
    float time = 0f;
    public MiniBossBaseAttack(Animator animator)
    {
        this.animator = animator;
    }
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        if (time > 1f)
        {
            time = 0f;
            return INode.NodeState.Success;
        }
        if(time == 0f)
            animator.SetTrigger("baseAttack");
        time += Time.deltaTime;
        return INode.NodeState.Running;
    }
}
