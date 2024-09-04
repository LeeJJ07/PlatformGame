using System.Collections.Generic;
using UnityEngine;


public class Sequence : INode
{
    List<INode> childNodes;
    int index = 0;
    string name = "";

    public Sequence()
    {
        childNodes = new List<INode>();
    }
    public Sequence(string name)
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
        if (childNodes == null || childNodes.Count == 0) 
            return INode.NodeState.Failure;
        if (index >= childNodes.Count)
        {
            index = 0;
            return INode.NodeState.Success;
        }
        switch (childNodes[index].Evaluate())
        {
            case INode.NodeState.Success:
                Debug.Log($"{childNodes[index].ToString()}: Success");
                index++;
                break;
                //리턴은 Failure일 때만
                

            case INode.NodeState.Failure:
                Debug.Log($"{childNodes[index].ToString()}: Failure");
                index = 0;
                return INode.NodeState.Failure;
        }


        return INode.NodeState.Running;
    }

    
}
