using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSequence : INode
{
    List<INode> childNodes;
    int randomState = 0;
    public RandomSequence(List<INode> childs)
    {
        childNodes = childs;
    }
    public RandomSequence()
    {
        childNodes = new List<INode>();
        
    }
    public void addNode(INode node)
    {
        childNodes.Add(node);
    }

    //랜덤 기능 추가 할 것
    public INode.NodeState Evaluate()
    {
        if (childNodes == null || childNodes.Count == 0)
            return INode.NodeState.Failure;
        Debug.Log("randomState: " + randomState);
        switch (childNodes[randomState].Evaluate())
        {
            case INode.NodeState.Running:
                return INode.NodeState.Running;

            case INode.NodeState.Success:
                randomState = Random.Range(0, childNodes.Count);
                break;

            case INode.NodeState.Failure:
                randomState = Random.Range(0, childNodes.Count);
                return INode.NodeState.Failure;
        }
     
        
        return INode.NodeState.Success;
    }
}
