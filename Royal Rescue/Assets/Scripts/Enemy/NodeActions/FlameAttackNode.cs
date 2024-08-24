using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class FlameAttackNode : INode
{
    FlameAttackScriptableObject flameAttackInfo;
    Animator aniController;
    Transform transform;
    Transform target;
    float animationDuration = 100;
    float skillActiveTime = 0;
    bool isActiveAnime = false;


    public FlameAttackNode(FlameAttackScriptableObject flameAttackInfo, Animator aniController, Transform transform, Transform target)
    {
        this.flameAttackInfo = flameAttackInfo;
        this.aniController = aniController;
        this.transform = transform;
        this.target = target;
    }
    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        if (!flameAttackInfo)
        {
            Debug.Log("FlameAttack Failure");
            return INode.NodeState.Failure;
        }
        skillActiveTime += Time.deltaTime;
        ActiveAnimation();
        if (skillActiveTime >= animationDuration)
        {
            Debug.Log("FlameAttack Success");
            
            isActiveAnime = false;
            skillActiveTime = 0;
            return INode.NodeState.Success;
        }

        return INode.NodeState.Running;
    }
    void ActiveAnimation()
    {
        if (isActiveAnime) return;
        aniController.SetTrigger("FlameAttackTrigger");
        if (aniController.GetCurrentAnimatorStateInfo(0).IsName("Flame Attack"))
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

