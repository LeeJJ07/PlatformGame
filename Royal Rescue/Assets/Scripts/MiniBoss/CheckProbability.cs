using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckProbability : INode
{
    int probability;
    public CheckProbability(int  probability)
    {
        this.probability = probability;
    }
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        int ran = Random.Range(0, 100);

        if (ran < probability)
            return INode.NodeState.Success;
        return INode.NodeState.Failure;
    }
}
