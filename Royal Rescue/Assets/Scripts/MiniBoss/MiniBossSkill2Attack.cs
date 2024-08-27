using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossSkill2Attack : INode
{
    public MiniBossSkill2Attack()
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
