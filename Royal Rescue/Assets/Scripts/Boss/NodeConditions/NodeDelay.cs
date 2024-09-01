using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NodeDelay;
using static UnityEngine.GraphicsBuffer;

public class NodeDelay : INode
{
    public delegate bool SetGetHitAni();
    public delegate void SetDelayTime(bool value);
    SetGetHitAni setGetHitAni;
    SetDelayTime setDelayTime;
    Animator aniController;
    float duration = 0;
    float span = 0;
    bool isActiveAnime = false;
    bool isActiveGetHitAni = false;
    public NodeDelay(SetGetHitAni setGetHitAni, SetDelayTime setDelayTime, float duration,Animator aniController)
    {
        this.setGetHitAni = setGetHitAni;
        this.setDelayTime = setDelayTime;
        this.aniController = aniController;
        this.duration = duration; 
    }

    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        ActiveAnimation();
        span += Time.deltaTime;
        if (span >= duration && !isActiveGetHitAni) 
        {
            span = 0;
            setGetHitAni();
            setDelayTime(false);
            isActiveAnime = false;
            return INode.NodeState.Success;
        }
        isActiveGetHitAni = setGetHitAni();
        return INode.NodeState.Running;
    }
    void ActiveAnimation()
    {
        if (isActiveAnime) return;
        
        aniController.SetBool("isWalk", false);
        if (aniController.GetCurrentAnimatorStateInfo(0).IsName("Idle01"))
        {
            Debug.Log("DelayTime");
            setDelayTime(true);
            isActiveAnime = true;
        }
    }
}
