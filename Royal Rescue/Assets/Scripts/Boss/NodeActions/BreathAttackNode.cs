using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BreathAttackNode : INode
{
    public delegate GameObject SpawnObject(GameObject obj, Vector3 spawnPosi);
    SpawnObject SpawnBreathParticle;
    BreathAttackScriptableObject breathAttackInfo;
    GameObject breathObj;
    Animator aniController;
    Transform particlSpawnPosi;
    Transform transform;
    Transform target;
    float skillStartTime = 0.5f;
    float skillEndTime = 100;
    float animationDuration = 100;
    float skillActiveSpan = 0;
    bool isStartParticle = false;
    bool isActiveAnime;
    public BreathAttackNode(SpawnObject SpawnBreathParticle, BreathAttackScriptableObject breathAttackInfo, Animator aniController, Transform particlSpawnPosi, Transform transform, Transform target)
    {
        this.SpawnBreathParticle = SpawnBreathParticle;
        this.breathAttackInfo = breathAttackInfo;
        this.aniController = aniController;
        this.particlSpawnPosi = particlSpawnPosi;
        this.transform = transform;
        this.target = target;
    }

    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        if (breathAttackInfo == null) 
        {
            Debug.Log("BreathAttack Failure");
            return INode.NodeState.Failure;
        }
        Debug.Log("BreathAttack Running");
        if (breathObj!=null)
            breathObj.transform.position = particlSpawnPosi.position;
        skillActiveSpan += Time.deltaTime;

        ActiveAnimation();
        if ((skillActiveSpan >= skillStartTime) && !isStartParticle)
        {
            breathObj.GetComponent<ParticleSystem>().Play();
            isStartParticle = true;
        }
        else if ((skillActiveSpan >= skillEndTime) && isStartParticle)
        {
            breathObj.GetComponent<ParticleSystem>().Stop();
        }


        if (skillActiveSpan >= animationDuration)
        {
            Debug.Log("BreathAttack Success");

            breathObj.SetActive(false);
            isStartParticle = false;
            isActiveAnime = false;
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
            breathObj = SpawnBreathParticle(breathAttackInfo.breathObj, particlSpawnPosi.position);

            Vector3 breathShootDir = target.position - breathObj.transform.position;

            breathObj.transform.rotation = Quaternion.LookRotation(breathShootDir.normalized);
            
            animationDuration = aniController.GetCurrentAnimatorStateInfo(0).length;
            skillEndTime = animationDuration - 0.6f;
            Vector3 dir = target.position - transform.position;
            if (dir.normalized.x < 0)
                transform.rotation = Quaternion.Euler(0, -90, 0);
            else
                transform.rotation = Quaternion.Euler(0, 90, 0);

            isActiveAnime = true;
        }
    }
}
