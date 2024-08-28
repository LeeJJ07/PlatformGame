using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPhase3Node : INode
{
    public delegate GameObject SpawnObj(GameObject obj, Vector3 posi);
    public delegate void DeActivateParticles();
    DeActivateParticles deActivateParticles;
    SpawnObj spawnObj;
    GameObject angryLight;
    Transform transform;
    Transform target;
    Transform spawnPosi;
    GameObject shockWaveParticleObj;
    GameObject FlameParticleObj;
    ParticleSystem shockWaveParticle;
    Animator aniController;
    float shockWaveStartTime = 1f;
    float shockWaveSpan = 0;

    float time = 0;
    float animationDuration = 100;
    bool isActiveAnime = false;
    bool isStartParticle = false;
    public EntryPhase3Node(GameObject light, Transform transform, Transform target,Transform spawnPosi,GameObject shockWave, GameObject FlameParticleObj,Animator aniController, SpawnObj spawnObj, DeActivateParticles deActivateParticles)
    {
        angryLight = light;
        this.transform = transform;
        this.target = target;
        this.shockWaveParticleObj = shockWave;
        this.aniController = aniController;
        this.spawnObj = spawnObj;
        this.spawnPosi = spawnPosi;
        this.FlameParticleObj = FlameParticleObj;
        this.deActivateParticles = deActivateParticles;
    }
    public void AddNode(INode node) { }

    public INode.NodeState Evaluate()
    {
        Debug.Log("entryPhase3 Running");
        time += Time.deltaTime;
        shockWaveSpan += Time.deltaTime;
        ActiveAnimation();
        Collider[] colliders = Physics.OverlapSphere(transform.position, 15, LayerMask.GetMask("Player"));
        if (colliders != null)
        {

            foreach (Collider collider in colliders)
            {
                Vector3 dir = collider.transform.position - transform.position;
                Mathf.Clamp(dir.x, 0, 1);
                Mathf.Clamp(dir.y, 0, 1);
                dir.z = 0;
                Rigidbody rigid = collider.GetComponent<Rigidbody>();
                if (rigid != null)
                {
                    rigid.AddForce(Vector3.right*dir.normalized.x/1.2f,ForceMode.Impulse);
                }
            }
        }
        if (shockWaveParticle != null)
            shockWaveParticle.transform.position = spawnPosi.position;
        if (shockWaveSpan >= shockWaveStartTime && !isStartParticle) 
        {
            isStartParticle=true;
            shockWaveParticle.Play();
        }

        if (time > animationDuration)
        {
            Debug.Log("entryPhase3 Success");
            shockWaveParticle.Stop();
            time = 0;
            shockWaveSpan = 0;
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

            deActivateParticles();
            shockWaveParticle = spawnObj(shockWaveParticleObj, spawnPosi.position).GetComponent<ParticleSystem>();
            Vector3 dir = target.position - transform.position;
            FlameParticleObj.SetActive(true);
            if (dir.normalized.x < 0)
                transform.rotation = Quaternion.Euler(0, -90, 0);
            else
                transform.rotation = Quaternion.Euler(0, 90, 0);

            angryLight.SetActive(true);
            isActiveAnime = true;
        }
    }
}
