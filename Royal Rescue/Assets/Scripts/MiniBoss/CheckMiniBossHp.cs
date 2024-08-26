using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMiniBossHp : INode
{
    public CheckMiniBossHp()
    {

    }
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        return INode.NodeState.Failure;
    }
}
