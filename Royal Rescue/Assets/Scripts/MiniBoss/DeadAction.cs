using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadAction : INode
{
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        throw new System.NotImplementedException();
    }
}
