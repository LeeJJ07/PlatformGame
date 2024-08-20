using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class CheckIncomingPhase2 : INode
{
    bool isIncoming = true;
    public void AddNode(INode node)
    {
    }

    public INode.NodeState Evaluate()
    {
        if(!isIncoming)
        {
            Debug.Log("incoming Failure");
            return INode.NodeState.Failure;
        }
        Debug.Log("incoming success");
        isIncoming = false;
        return INode.NodeState.Success;
    }
}
