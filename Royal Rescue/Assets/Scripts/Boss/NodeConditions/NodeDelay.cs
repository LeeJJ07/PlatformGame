using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NodeDelay;
using static UnityEngine.GraphicsBuffer;

public class NodeDelay : INode
{
    public delegate bool GetGetHitAni();
    public delegate void SetDelayTime(bool value);
    GetGetHitAni setGetHitAni;
    SetDelayTime setDelayTime;
    Animator aniController;
    float duration = 0;
    float span = 0;
    bool isActiveAnime = false;
    bool isActiveGetHitAni = false;
    public NodeDelay(GetGetHitAni setGetHitAni, SetDelayTime setDelayTime, float duration,Animator aniController)
    {
        this.setGetHitAni = setGetHitAni;
        this.setDelayTime = setDelayTime;
        this.aniController = aniController;
        this.duration = duration;
    }

    public void AddNode(INode node) { }

    /*
     * �̽� : ������ Delay��尡 ������ 0.1������ �÷��̾��� ������ ���� ��
     * �����ڵ尡 �ƿ� ��������� �ɰ��� �̽� �߰ߵ�
     */
    public INode.NodeState Evaluate()
    {
        ActiveAnimation();
        span += Time.deltaTime;
        if (span >= duration && !isActiveGetHitAni) 
        {
            Debug.Log($"delayEnd");
            span = 0;
            isActiveAnime = false;
            setDelayTime(false);
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
            isActiveAnime = true;
            setDelayTime(true);
        }
    }
}
