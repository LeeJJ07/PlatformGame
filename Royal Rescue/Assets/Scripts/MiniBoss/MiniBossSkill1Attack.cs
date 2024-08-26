using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossSkill1Attack : INode
{
    public MiniBossSkill1Attack()
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
