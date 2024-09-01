using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NodeDelay : INode
{
    Animator aniController;
    float duration = 0;
    float span = 0;
    bool isActiveAnime = false;
    public NodeDelay(float duration,Animator aniController)
    {
        this.aniController = aniController;
        this.duration = duration; 
    }

    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        ActiveAnimation();
        span += Time.deltaTime;
        
        if (span >= duration) 
        {
            span = 0;
            return INode.NodeState.Success;
        }
        return INode.NodeState.Running;
    }
    void ActiveAnimation()
    {
        if (isActiveAnime) return;
        aniController.SetBool("isWalk", false);
        if (aniController.GetCurrentAnimatorStateInfo(0).IsName("Idle01"))
        {
            isActiveAnime = true;
        }
    }
}
