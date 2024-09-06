using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieNode : INode
{
    public delegate void DeActiveObj();
    DeActiveObj deActiveObj;
    Transform transform;
    Transform target;
    Animator aniController;

    float animationDuration = 100;
    float time = 0;
    bool isActiveAnime = false;
    
    public DieNode(DeActiveObj deActiveObj, Transform transform, Transform target, Animator aniController)
    {
        this.deActiveObj = deActiveObj;
        this.transform = transform;
        this.target = target;
        this.aniController = aniController;
    }
    public void AddNode(INode node) {}

    public INode.NodeState Evaluate()
    {
        ActiveAnimation();
        time += Time.deltaTime;
        Debug.Log("Die Running");
        if (time>animationDuration)
        {
            Debug.Log("Die Success");
            animationDuration = 0;
            isActiveAnime = false;
            
            return INode.NodeState.Success;
        }
        return INode.NodeState.Running;
    }
    void ActiveAnimation()
    {
        if (isActiveAnime) return;
        aniController.SetTrigger("DieTrigger");
        if (aniController.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            deActiveObj();
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
