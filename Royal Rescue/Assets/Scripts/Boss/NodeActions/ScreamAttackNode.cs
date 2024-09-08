using UnityEngine;

public class ScreamAttackNode : INode
{

    //적 랜덤스폰 함수
    public delegate void SpawnRandomObjs(GameObject[] objs,int count);
    
    //파티클 스폰 함수
    public delegate GameObject SpawnObjs(GameObject objs, Vector3 spawnPosi);

    SpawnRandomObjs SpawnRandomObject;
    SpawnObjs SpawnObject;
    ScreamAttackScriptableObject screamAttackInfo;
    Animator aniController;

    GameObject shockWaveObj;
    Transform particlePosi;
    float animationDuration = 100;
    float shockWaveStartTime = 1f;
    float shockWaveSpan = 0;
    float time = 0;

    bool isActiveAnime = false;
    bool isStartParticle = false;
    bool isSpawnEnemy = false;

    public ScreamAttackNode(ScreamAttackScriptableObject screamAttackInfo, SpawnRandomObjs SpawnRandomObject,SpawnObjs SpawnObject, Animator aniController,Transform particlePosi)
    {
        this.screamAttackInfo = screamAttackInfo;
        this.aniController = aniController;
        this.SpawnRandomObject = SpawnRandomObject;
        this.SpawnObject = SpawnObject;
        this.particlePosi = particlePosi;
    }
    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        if (!screamAttackInfo) 
            return INode.NodeState.Failure;
        
        ActiveAnimation();
        objectSpawn();
        time += Time.deltaTime;
        shockWaveSpan += Time.deltaTime;
        if (shockWaveObj != null)
            shockWaveObj.transform.position = particlePosi.position;
        if(shockWaveSpan>=shockWaveStartTime&&!isStartParticle)
        {
            shockWaveObj.GetComponent<ParticleSystem>().Play();
            isStartParticle = true;
        }
        if (time >= animationDuration)
        {
            shockWaveObj.GetComponent<ParticleSystem>().Stop();
            shockWaveObj.SetActive(false);
            Debug.Log("Scream Success");

            shockWaveSpan = 0;
            time = 0;
            isStartParticle = false;
            isSpawnEnemy = false;
            isActiveAnime = false;
            return INode.NodeState.Success;
        }
        return INode.NodeState.Running;
    }
    void objectSpawn()
    {
        if (isSpawnEnemy) return;
        SpawnRandomObject(screamAttackInfo.objs,screamAttackInfo.objSpawnCount);
        isSpawnEnemy = true;
    }
    void ActiveAnimation()
    {
        if (isActiveAnime) return;
        aniController.SetTrigger("ScreamTrigger");
        if (aniController.GetCurrentAnimatorStateInfo(0).IsName("Scream"))
        {
            animationDuration = aniController.GetCurrentAnimatorStateInfo(0).length;
            shockWaveObj = SpawnObject(screamAttackInfo.shockWaveObj, particlePosi.position);
            isActiveAnime = true;
        }
    }
    
}
