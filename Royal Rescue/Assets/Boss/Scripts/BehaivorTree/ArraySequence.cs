using System.Collections.Generic;
using UnityEngine;

public class ArraySequence : INode
{
    List<INode> nodes;
    int index = 0;
    public ArraySequence()
    {
        nodes = new List<INode>();
    }
    public void AddNode(INode node)
    {
        nodes.Add(node);
    }
    public INode.NodeState Evaluate()
    {
        Debug.Log(index);
        switch (nodes[index].Evaluate())
        {
            case INode.NodeState.Success:
                index++;
                break;
            case INode.NodeState.Failure:
                
                index = 0;
                break;
            case INode.NodeState.Running:
                break;
                
        }
        if(index>=nodes.Count-1)
        {
            index = 0;
            return INode.NodeState.Failure;
        }
        
        return INode.NodeState.Running;
    }
    
}
