using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class FlameAttackNode : INode
{
    //사운드 재생 함수
    public delegate void SoundEffect(string name, bool isLoop);
    public delegate GameObject SpawnObj(GameObject obj, Vector3 posi);
    SpawnObj SpawnFlame;
    SoundEffect playSound;
    FlameAttackScriptableObject flameAttackInfo;
    Transform flameSpawntransform;
    Animator aniController;
    Transform transform;
    Transform target;
    float flameShootStartTime = 0.5f;
    float shootGap = 0;
    float startShootTime = 100;
    float animationDuration = 100;
    float skillActiveSpan = 0;
    int shootCount = 0;

    bool isActiveAnime = false;


    public FlameAttackNode(FlameAttackScriptableObject flameAttackInfo,SpawnObj SpawnFlame,Transform flameSpawntransform, Animator aniController, Transform transform, Transform target, SoundEffect playSound)
    {
        this.flameAttackInfo = flameAttackInfo;
        this.SpawnFlame = SpawnFlame;
        this.flameSpawntransform = flameSpawntransform;
        this.aniController = aniController;
        this.transform = transform;
        this.target = target;
        this.playSound = playSound;
    }
    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        if (!flameAttackInfo)
        {
            Debug.Log("FlameAttack Failure");
            return INode.NodeState.Failure;
        }
        ActiveAnimation();
        skillActiveSpan += Time.deltaTime;
        if (skillActiveSpan >= startShootTime&& shootCount<flameAttackInfo.flameCount)
        {
            GameObject flame = SpawnFlame(flameAttackInfo.flameObj, flameSpawntransform.position);
            flame.GetComponent<FlameBehavior>().SetDamage(flameAttackInfo.damage);
            playSound(flameAttackInfo.soundClipName, false);
            startShootTime += shootGap;
            shootCount++;
        }
        if (skillActiveSpan >= animationDuration)
        {
            Debug.Log("FlameAttack Success");
            isActiveAnime = false;
            startShootTime = 100;
            shootCount = 0;
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
            shootGap = (animationDuration - flameShootStartTime-1f) / (float)(flameAttackInfo.flameCount);
            startShootTime = flameShootStartTime;
            float dir = (target.position.x - transform.position.x)/Mathf.Abs((target.position.x - transform.position.x));

            transform.rotation = Quaternion.Euler(0, 90*dir, 0);

            isActiveAnime = true;
        }
    }
}

