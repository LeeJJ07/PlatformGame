using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Phase1AttackArrSequence : INode
{
    List<INode> nodes;
    List<List<int>> PatternList;
    EnemyAI enemyAI;
    int index = 0;
    public Phase1AttackArrSequence(EnemyAI enemyAI, List<List<int>> PatternList)
    {
        nodes = new List<INode>();
        this.enemyAI = enemyAI;
        this.PatternList = PatternList;
    }
    public void AddNode(INode node)
    {
        nodes.Add(node);
    }

    /*
     TODO: 왜 돌아가는 거죠...? 
     */
    public INode.NodeState Evaluate()
    {
        if (!enemyAI)
            return INode.NodeState.Failure;
        if (index >= PatternList[enemyAI.AttackIndex].Count)
        {
            index = 0;
            return INode.NodeState.Failure;
        }

        switch (nodes[PatternList[enemyAI.AttackIndex][index]].Evaluate())
        {
            case INode.NodeState.Success:
            case INode.NodeState.Failure:
                index++;
                return INode.NodeState.Success;
            case INode.NodeState.Running:
                break;
        }
        Debug.Log("PatternList[enemyAI.AttackIndex]: " + PatternList[enemyAI.AttackIndex].Count);
        Debug.Log("patternIndex: " + index);
       
                
        return INode.NodeState.Running;
    }
}
