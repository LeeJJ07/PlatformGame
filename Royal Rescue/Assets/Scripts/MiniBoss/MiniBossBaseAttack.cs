using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossBaseAttack : INode
{
    public MiniBossBaseAttack()
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
