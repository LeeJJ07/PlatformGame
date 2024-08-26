using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckProbability : INode
{
    public CheckProbability()
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
