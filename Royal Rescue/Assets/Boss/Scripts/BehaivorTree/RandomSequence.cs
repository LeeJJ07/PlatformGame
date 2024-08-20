
using System.Collections.Generic;
using UnityEngine;

public class RandomSequence : INode
{
    List<INode> childNodes;
    int randomStateIndex = 0;
    bool isSelectSkill = false;

    string name = "";
    public RandomSequence(List<INode> childs)
    {
        childNodes = childs;
    }
    public RandomSequence()
    {
        childNodes = new List<INode>();
        
    }
    public RandomSequence(string name)
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
        Debug.Log(name);
        Debug.Log("randomState: " + randomStateIndex);
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
