using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHitActionNode : INode
{
    Transform transform;
    Transform target;
    Animator aniController;
    float animationDuration = 100f;
    float span = 0;

    bool isActiveAnime = false;

    public GetHitActionNode(Animator aniController, Transform transform, Transform target)
    {
        this.aniController = aniController;
        this.transform = transform;
        this.target = target;
    }
    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        ActiveAnimation();
        span += Time.deltaTime;
        if(span>=animationDuration)
        {
            span = 0;
            isActiveAnime = false;
            return INode.NodeState.Success;
        }
        return INode.NodeState.Running;
    }
    void ActiveAnimation()
    {
        if (isActiveAnime) return;
        aniController.SetTrigger("GetHitTrigger");
        if (aniController.GetCurrentAnimatorStateInfo(0).IsName("Get Hit"))
        {
            animationDuration = aniController.GetCurrentAnimatorStateInfo(0).length;
            Vector3 dir = target.position - transform.position;
            dir.z = 0;
            dir.y = 0;
            if (dir.normalized.x < 0)
                transform.rotation = Quaternion.Euler(0, -90, 0);
            else
                transform.rotation = Quaternion.Euler(0, 90, 0);


            isActiveAnime = true;
        }
    }
}
