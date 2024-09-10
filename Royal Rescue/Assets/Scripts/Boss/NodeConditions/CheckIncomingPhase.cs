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
        {
            Debug.Log("incoming Failure");
            return INode.NodeState.Failure;
        }
        Debug.Log("incoming success");
        isIncoming = false;
        return INode.NodeState.Success;
    }
}
