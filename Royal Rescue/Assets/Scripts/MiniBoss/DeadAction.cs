using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadAction : INode
{
    public DeadAction()
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
