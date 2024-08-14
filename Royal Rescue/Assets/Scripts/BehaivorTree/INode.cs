public interface INode
{
    enum NodeState { Success, Failure, Running };
    public INode.NodeState Evaluate();
    
}
