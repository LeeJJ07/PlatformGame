using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : INode
{
    private Transform playerPos;
    private Transform startPlayerPosition;
    public DetectPlayer(Transform playerPos, Transform startPlayerPosition)
    {
        this.playerPos = playerPos;
        this.startPlayerPosition = startPlayerPosition;
    }
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        if (playerPos.position.x >= startPlayerPosition.position.x)
            return INode.NodeState.Failure;
        return INode.NodeState.Success;
    }
}
