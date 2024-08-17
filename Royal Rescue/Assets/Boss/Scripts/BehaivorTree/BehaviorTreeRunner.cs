using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeRunner
{
    INode root;
    public BehaviorTreeRunner(INode root)
    {
        this.root = root;
    }
    public void Operator()
    {
        root.Evaluate();
    }
}
