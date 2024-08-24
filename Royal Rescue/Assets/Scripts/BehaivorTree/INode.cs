public interface INode
{
    enum NodeState { Success, Failure, Running };
    public INode.NodeState Evaluate();
    public void AddNode(INode node);

}
