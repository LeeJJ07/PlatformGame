using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Phase1ActionSelector : INode
{
    List<INode> nodes;
    int index = 0;
    int res = 1;
    public Phase1ActionSelector()
    {
        nodes = new List<INode>();
    }
    public void AddNode(INode node)
    {
        nodes.Add(node);
    }

    public INode.NodeState Evaluate()
    {
        if (nodes.Count <= 0)
            return INode.NodeState.Failure;

        switch (nodes[index].Evaluate())
        {
            case INode.NodeState.Success:
                res = res | 1;
                index++;
                break;

            case INode.NodeState.Failure:
                res = res | 0;
                index++;
                break;

            case INode.NodeState.Running:
                break;
        }
        if (index >= nodes.Count)
        {
            index = 0;
            if(res==1)
                return INode.NodeState.Success;
            else
                return INode.NodeState.Failure;
        }
        return INode.NodeState.Running;
    }
}
