using System.Collections.Generic;

public class Sequence : INode
{
    List<INode> childNodes;
    int index = 0;
    bool abortState = false;
   
    public Sequence()
    {
        childNodes = new List<INode>();
    }
    public void addNode(INode node)
    {
        childNodes.Add(node);
    }
    
    public void abort(bool isAbort)
    {
        abortState = isAbort;
    }
    
    public INode.NodeState Evaluate()
    {
        if (childNodes == null || childNodes.Count == 0) 
            return INode.NodeState.Failure;

        if (index >= childNodes.Count) 
            index = 0;
        if (abortState)
        {
            index = 0;
            return INode.NodeState.Failure;
        }

        switch (childNodes[index].Evaluate())
        {
            case INode.NodeState.Running:
                return INode.NodeState.Running;

            case INode.NodeState.Success:
                index++;
                break;

            case INode.NodeState.Failure:
                index = 0;
                return INode.NodeState.Failure;
        }


        return INode.NodeState.Success;
    }
}
