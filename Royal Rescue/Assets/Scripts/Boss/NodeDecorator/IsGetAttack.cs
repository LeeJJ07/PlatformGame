using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IsGetAttack : INode
{
    public delegate bool GetIsHit();
    public delegate void SetIsHit(bool value);
    GetIsHit isGetDamage;
    SetIsHit setHit;
    INode node;
    public IsGetAttack(GetIsHit isGetDamage, SetIsHit setHit)
    {
        this.isGetDamage = isGetDamage;
        this.setHit = setHit;
    }
    public void AddNode(INode node)
    {
        this.node = node;
    }

    public INode.NodeState Evaluate()
    {
        if (!isGetDamage())
            return INode.NodeState.Failure;

        switch(node.Evaluate())
        {
            case INode.NodeState.Failure:
                setHit(false);
                return INode.NodeState.Failure;
            case INode.NodeState.Success:
                setHit(false);
                return INode.NodeState.Success;
        }
        return INode.NodeState.Running;
    }
}
