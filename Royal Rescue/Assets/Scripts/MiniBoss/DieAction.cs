using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAction : INode
{
    public delegate void Die();
    Die die;
    float time = 0f;

    Animator animator;
    public DieAction(Die die, Animator animator)
    {
        this.die = die;
        this.animator = animator;
    }
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        if(time > 2f)
        {
            die();
            return INode.NodeState.Success;
        }
        if (time == 0f) animator.SetTrigger("die");
        time += Time.deltaTime;
        return INode.NodeState.Running;
    }
}
