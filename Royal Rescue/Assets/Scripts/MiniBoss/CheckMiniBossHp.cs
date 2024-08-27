using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMiniBossHp : INode
{
    public delegate float Hp();
    Hp hp;
    public CheckMiniBossHp(Hp hp)
    {
        this.hp = hp;
    }
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        if (hp() <= 0f)
            return INode.NodeState.Success;
        return INode.NodeState.Failure;
    }
}
