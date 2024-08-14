using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Selector : INode
{
    List<INode> childNodes;
    int index = 0;
    
    public Selector() 
    {
        childNodes = new List<INode>();
    }
    public void addNode(INode node)
    {
        childNodes.Add(node);
    }
    public INode.NodeState Evaluate()
    {
        if (childNodes == null)
            return INode.NodeState.Failure;

        if (index >= childNodes.Count)
            index = 0;

        switch (childNodes[index].Evaluate())
        {
            case INode.NodeState.Running:
                return INode.NodeState.Running;
            case INode.NodeState.Success:
                index++;
                return INode.NodeState.Success;
            case INode.NodeState.Failure:
                index = 0;
                return INode.NodeState.Failure;
        }
        
        


        return INode.NodeState.Running;
    }
}
