using System.Collections;
using UnityEngine;
public class FlameBehavior : MonoBehaviour
{
    [SerializeField] ParticleSystem flameParticle;
    [SerializeField] ParticleSystem explosionParticle;
    [SerializeField] float moveSpeed;
    Coroutine skillCoroutine;
    Transform target;
    bool isPlayExposion = false;
    Vector3 dir;
    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void ActiveSkill()
    {   
        flameParticle.Play();
        explosionParticle.Stop();
        dir = target.position - transform.position;
        skillCoroutine = StartCoroutine("ActiveFlameSkill");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Skill")) 
            return;
        StopCoroutine(skillCoroutine);
        flameParticle.Stop();
        if(!isPlayExposion)
        {
            explosionParticle.Play();
            isPlayExposion = true;
        }
        Destroy(gameObject, 2f);
    }
    IEnumerator ActiveFlameSkill()
    {
        while(true)
        {
            
            transform.position += dir.normalized * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }
    
}
