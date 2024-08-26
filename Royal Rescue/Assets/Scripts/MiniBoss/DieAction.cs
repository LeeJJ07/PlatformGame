using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAction : INode
{
    public delegate void Die();
    Die die;

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
        die();
        return INode.NodeState.Success;
    }
}
