using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase1ActionSequcne : INode
{
    List<INode> nodes;
    int index = 0;
    public void AddNode(INode node)
    {
        nodes.Add(node);
    }

    public INode.NodeState Evaluate()
    {
        if (nodes.Count <= 0) 
            return INode.NodeState.Failure;

        switch(nodes[index].Evaluate())
        {
            case INode.NodeState.Success:
                index++;
                return INode.NodeState.Success;

            case INode.NodeState.Failure:
                index = 0;
                break;

            case INode.NodeState.Running:
                break;
        }
        return INode.NodeState.Running;
    }
}
