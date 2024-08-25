using UnityEngine;

public class ScreamAttackNode : INode
{
    public delegate void SpawnObjs(GameObject[] objs,int count);
    SpawnObjs SpawnEnemies;
    ScreamAttackScriptableObject screamAttackInfo;
    Animator aniController;
    ParticleSystem shockWave;
    Transform transform;
    Transform target;
    float animationDuration = 100;
    float shockWaveStartTime = 1f;
    float shockWaveSpan = 0;
    float time = 0;

    bool isActiveAnime = false;
    bool isStartParticle = false;
    bool isSpawnEnemy = false;

    public ScreamAttackNode(ScreamAttackScriptableObject screamAttackInfo,Animator aniController,ParticleSystem shockWave, SpawnObjs spawnEnemies,Transform transform, Transform target)
    {
        this.screamAttackInfo = screamAttackInfo;
        this.aniController = aniController;
        this.shockWave = shockWave;
        SpawnEnemies = spawnEnemies;
        this.transform = transform;
        this.target = target;
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
        if(shockWaveSpan>=shockWaveStartTime&&!isStartParticle)
        {
            shockWave.Play();
            isStartParticle = true;
        }
        if (time >= animationDuration)
        {
            shockWave.Stop();
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
        SpawnEnemies(screamAttackInfo.objs,screamAttackInfo.objSpawnCount);
        isSpawnEnemy = true;
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
