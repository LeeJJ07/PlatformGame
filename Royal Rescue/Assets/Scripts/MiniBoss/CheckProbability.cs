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
        float temp = Time.time * 100f;
        Random.InitState((int)temp);
        int ran = Random.Range(0, 100);

        if (ran < probability)
            return INode.NodeState.Failure;
        return INode.NodeState.Success;
    }
}
