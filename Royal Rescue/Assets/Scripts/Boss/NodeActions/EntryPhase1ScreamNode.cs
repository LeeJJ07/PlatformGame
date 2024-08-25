using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPhase1ScreamNode : INode
{
    Transform transform;
    Transform target;
    Animator aniController;
    GameObject shockWaveObj;
    GameObject shockWaveParticleObj;
    float shockWaveStartTime = 1f;
    float shockWaveSpan = 0;
    float time = 0;
    float animationDuration = 100;
    bool isStartParticle = false;
    bool isActiveAnime = false;

    //오브젝트 스폰할 델리게이트 매개변수 생성, ScriptableObject 고려해보기
    public EntryPhase1ScreamNode(Transform transform, Transform target, Animator aniController, GameObject shockWave)
    {
        this.transform = transform;
        this.target = target;
        this.aniController = aniController;
        this.shockWaveObj = shockWave;
    }
    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        ActiveAnimation();
        time += Time.deltaTime;
        shockWaveSpan += Time.deltaTime;
        if ((shockWaveSpan>=shockWaveStartTime)&& !isStartParticle)
        {
            Debug.Log("play particle");
            shockWaveObj.GetComponent<ParticleSystem>().Play();
            isStartParticle = true;
        }
        if (time >= animationDuration)
        {
            shockWaveObj.GetComponent<ParticleSystem>().Stop();
            shockWaveSpan = 0;
            time = 0;
            isStartParticle = false;
            isActiveAnime = false;
            return INode.NodeState.Success;
        }
        return INode.NodeState.Running;
    }
    void ActiveAnimation()
    {
        if (isActiveAnime) return;
        aniController.SetTrigger("ScreamTrigger");

        if (aniController.GetCurrentAnimatorStateInfo(0).IsName("Scream"))
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
