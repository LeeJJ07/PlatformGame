using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class CheckIncomingPhase : INode
{
    bool isIncoming = true;
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        if(!isIncoming)
            return INode.NodeState.Failure;
        
        
        isIncoming = false;
        return INode.NodeState.Success;
    }
}
