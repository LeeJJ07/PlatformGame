
using System.Collections.Generic;
using UnityEngine;

public class RandomSelector : INode
{
    List<INode> childNodes;
    int randomStateIndex = 0;
    bool isSelectSkill = false;

    string name = "";
    public RandomSelector(List<INode> childs)
    {
        childNodes = childs;
    }
    public RandomSelector()
    {
        childNodes = new List<INode>();
        
    }
    public RandomSelector(string name)
    {
        childNodes = new List<INode>();
        this.name = name;
    }
    public void AddNode(INode node)
    {
        childNodes.Add(node);
    }

    //랜덤 기능 추가 할 것
    public INode.NodeState Evaluate()
    {
        if (childNodes == null || childNodes.Count == 0)
            return INode.NodeState.Failure;
        SkillSelect();
        
        switch (childNodes[randomStateIndex].Evaluate())
        {
            case INode.NodeState.Success:
                isSelectSkill = false;
                return INode.NodeState.Success;

            case INode.NodeState.Failure:
                isSelectSkill = false;
                return INode.NodeState.Failure;
        }

        return INode.NodeState.Running;
    }

    void SkillSelect()
    {
        if (isSelectSkill) return;
        isSelectSkill = true;
        randomStateIndex = Random.Range(0, childNodes.Count);
    }
}
