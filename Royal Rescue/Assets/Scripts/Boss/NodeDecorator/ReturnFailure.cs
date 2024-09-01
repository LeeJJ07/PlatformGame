using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnFailure : INode
{
    INode node;
    public void AddNode(INode node)
    {
        this.node = node;
    }

    public INode.NodeState Evaluate()
    {
        switch(node.Evaluate())
        {
            case INode.NodeState.Success:
            case INode.NodeState.Failure:
                return INode.NodeState.Failure;
        }
        return INode.NodeState.Running;
    }
}
