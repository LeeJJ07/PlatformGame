using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Selector : INode
{
    List<INode> childNodes;
    int index = 0;
    string name = "";
    public Selector() 
    {
        childNodes = new List<INode>();
    }
    public Selector(string name)
    {
        childNodes = new List<INode>();
        this.name = name;
    }
    public void AddNode(INode node)
    {
        childNodes.Add(node);
    }

    public INode.NodeState Evaluate()
    {
        if (childNodes == null)
            return INode.NodeState.Failure;
        
        if (index >= childNodes.Count)
        {
            index = 0;
            return INode.NodeState.Failure;
        }

        switch (childNodes[index].Evaluate())
        {
            case INode.NodeState.Success:
                index = 0;
                return INode.NodeState.Success;

            case INode.NodeState.Failure:
                index++;
                break;
        }
        return INode.NodeState.Running;
    }
}
