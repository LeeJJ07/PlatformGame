using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : INode
{
    List<INode> childNodes;
    int index = 0;
    bool isSuccess = true;
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
        if (name.Equals("phase2ActionSelector"))
            Debug.Log("phase2ActionSelector: " + index);
        if (index >= childNodes.Count)
        {
            index = 0;
            if(isSuccess)
            {
                Debug.Log(name +" "+index+ ": success");
                return INode.NodeState.Success;
            }
            else
            {
                Debug.Log(name + " " + index + ": failure");
                return INode.NodeState.Failure;
            }
        }

        switch (childNodes[index].Evaluate())
        {
            case INode.NodeState.Success:
                isSuccess |= true;
                index++;
                break;
            case INode.NodeState.Failure:
                isSuccess |= false;
                index++;
                break;
        }



        return INode.NodeState.Running;
    }
}
