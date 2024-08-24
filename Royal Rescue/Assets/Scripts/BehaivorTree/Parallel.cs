using System.Collections.Generic;

public class Parallel : INode
{
    List<INode> nodes;
    string name = "";
    public Parallel()
    {
        nodes = new List<INode>();
    }
    public Parallel(string name)
    {
        nodes = new List<INode>();
        this.name = name;
    }
    public void AddNode(INode node)
    {
        nodes.Add(node);
    }

    public INode.NodeState Evaluate()
    {
        foreach(INode node in nodes)
        {
            switch(node.Evaluate())
            {
                case INode.NodeState.Success:
                    break;
                case INode.NodeState.Failure:
                    return INode.NodeState.Failure;
            }
        }
        return INode.NodeState.Running;
    }
}
