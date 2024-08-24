using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EntryPhase1LandNode : INode
{
    Transform transform;
    Transform target;
    Animator aniController;
    float time = 0;
    float animationDuration = 100;
    bool isActiveAnime = false;
    public EntryPhase1LandNode(Transform transform, Transform target, Animator aniController)
    {
        this.transform = transform;
        this.target = target;
        this.aniController = aniController;
    }
    public void AddNode(INode node) {}

    public INode.NodeState Evaluate()
    {
        ActiveAnimation();
        time += Time.deltaTime;
        if (time>=animationDuration)
        {
            time = 0;
            isActiveAnime = false;
            return INode.NodeState.Success;
        }
        return INode.NodeState.Running;
    }
    void ActiveAnimation()
    {
        if (isActiveAnime) return;
        aniController.SetTrigger("IntroTrigger");
        
        if (aniController.GetCurrentAnimatorStateInfo(0).IsName("Land"))
        {
            animationDuration = aniController.GetCurrentAnimatorStateInfo(0).length;
            Vector3 dir = target.position - transform.position;
            if (dir.normalized.x < 0)
                transform.rotation = Quaternion.Euler(0, -90, 0);
            else
                transform.rotation = Quaternion.Euler(0, 90, 0);


            isActiveAnime = true;
        }
    }
}
