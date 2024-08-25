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
    public void OnEnable()
    {
        target = GameObject.FindWithTag("Player").transform;
        flameParticle.Play();
        explosionParticle.Stop();
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
        gameObject.SetActive(false);
    }
    IEnumerator ActiveFlameSkill()
    {
        yield return new WaitForSeconds(0.1f);
        dir = target.position - transform.position;
        while (true)
        {
            
            transform.position += dir.normalized * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }
    
}
