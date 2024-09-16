using System.Collections;
using UnityEngine;
public class FlameBehavior : MonoBehaviour,ITag
{
    [Header("Detail Tag"), SerializeField]
    string detailTag = "";
    [SerializeField] ParticleSystem flameParticle;
    [SerializeField] ParticleSystem explosionParticle;
    [SerializeField] string[] ignoreTagList;
    [SerializeField] float moveSpeed;
    [SerializeField] float deActiveTime = 0;
    int damage = 0;
    Coroutine skillCoroutine;
    Transform target;
    Vector3 dir;
    bool isExplosion = false;
    public void OnEnable()
    {
        target = GameObject.FindWithTag("Player").transform;
        flameParticle.Play();
        explosionParticle.Stop();
        skillCoroutine = StartCoroutine("ActiveFlameSkill");
        StartCoroutine("DeActiveFlameSkill");
    }
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        if((other.CompareTag("Player")))
        {
            StopCoroutine(skillCoroutine);
            flameParticle.Stop();
            if (!isExplosion)
            {
                Debug.Log("Player Hit Flame");
                other.GetComponent<PlayerControlManagerFix>().HurtPlayer(damage);
                explosionParticle.Play();
                isExplosion = true;
            }
            

            StartCoroutine("WaitDeActiveFlameSkill");
        }
        else if(other.CompareTag("Floor"))
        {
            StopCoroutine(skillCoroutine);
            flameParticle.Stop();
            if (!isExplosion)
            {
                explosionParticle.Play();
                isExplosion = true;
            }
            StartCoroutine("WaitDeActiveFlameSkill");

        }
    }
    IEnumerator DeActiveFlameSkill()
    {
        yield return new WaitForSeconds(deActiveTime);
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
    IEnumerator WaitDeActiveFlameSkill()
    {
        yield return new WaitForSeconds(explosionParticle.main.duration);
        
        explosionParticle.Stop();
        isExplosion = false;
        gameObject.SetActive(false);
    }

    public string GetTag()
    {
        return detailTag;
    }

    public bool CompareToTag(string detailTag)
    {
        return this.detailTag == detailTag;
    }
}
