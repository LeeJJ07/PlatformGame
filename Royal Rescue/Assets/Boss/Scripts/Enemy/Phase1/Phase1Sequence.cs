using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase1Sequence : INode
{
    EnemyAI enemyAI;
    List<INode> nodes;
    int index = 0;
    public Phase1Sequence(EnemyAI enemyAI)
    {
        nodes = new List<INode>();
        this.enemyAI = enemyAI;
    }
    public void AddNode(INode node)
    {
        nodes.Add(node);
    }
    public INode.NodeState Evaluate()
    {
        if (enemyAI.Hp < enemyAI.Phase1HpCondition1)
            return INode.NodeState.Failure;

        switch(nodes[index].Evaluate())
        {
            case INode.NodeState.Success:
                index++;
                break;
            case INode.NodeState.Running:
                break;
            case INode.NodeState.Failure:
                index = 0;
                break;
        }
        if(index>=nodes.Count)
        {
            index = 0;
        }
        return INode.NodeState.Running;
    }
}
