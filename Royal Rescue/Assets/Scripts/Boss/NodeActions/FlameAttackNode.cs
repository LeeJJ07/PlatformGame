using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class FlameAttackNode : INode
{
    public delegate void SpawnObj(GameObject obj, Vector3 posi);
    SpawnObj SpawnFlame;
    FlameAttackScriptableObject flameAttackInfo;
    Transform flameSpawntransform;
    Animator aniController;
    Transform transform;
    Transform target;
    float flameShootStartTime = 0.2f;
    float shootGap = 0;
    float shootTime = 0;
    float animationDuration = 100;
    float skillActiveSpan = 0;
    bool isActiveAnime = false;


    public FlameAttackNode(FlameAttackScriptableObject flameAttackInfo,SpawnObj SpawnFlame,Transform flameSpawntransform, Animator aniController, Transform transform, Transform target)
    {
        this.flameAttackInfo = flameAttackInfo;
        this.SpawnFlame = SpawnFlame;
        this.flameSpawntransform = flameSpawntransform;
        this.aniController = aniController;
        this.transform = transform;
        this.target = target;
        shootTime = flameShootStartTime;
    }
    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        if (!flameAttackInfo)
        {
            Debug.Log("FlameAttack Failure");
            return INode.NodeState.Failure;
        }
        skillActiveSpan += Time.deltaTime;
        ActiveAnimation();
        if (skillActiveSpan >= shootTime)
        {
            SpawnFlame(flameAttackInfo.flameObj, flameSpawntransform.position);
            shootTime += shootGap;
        }
        if (skillActiveSpan >= animationDuration)
        {
            Debug.Log("FlameAttack Success");
            
            isActiveAnime = false;
            shootTime = flameShootStartTime;
            skillActiveSpan = 0;
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
            shootGap = animationDuration / (float)(flameAttackInfo.flameCount+1);
            Vector3 dir = target.position - transform.position;
            if (dir.normalized.x < 0)
                transform.rotation = Quaternion.Euler(0, -90, 0);
            else
                transform.rotation = Quaternion.Euler(0, 90, 0);


            isActiveAnime = true;
        }
    }
}

