using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerTrap : MonoBehaviour
{
    [SerializeField] private float interval;
    [SerializeField] private float hitInterval;
    [SerializeField] private int damage;
    [SerializeField] private ParticleSystem effect;
    [SerializeField] private GameObject hitEffect;
    private float hitTimer;

    void OnEnable()
    {
        StartCoroutine(FlameThrower());
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }

    void OnParticleCollision(GameObject other)
    {
        if (hitTimer == 0f)
        {
            GameDirector.instance.PlayerControl.HurtPlayer(damage);
            GameObject hitEf = Instantiate(hitEffect, transform);
            hitEf.transform.position = GameDirector.instance.PlayerControl.transform.position;
            hitEf.GetComponent<ParticleSystem>().Play();
            Destroy(hitEf, 0.4f);
        }
        else if (hitTimer >= hitInterval)
            hitTimer = 0f;

        hitTimer += Time.deltaTime;
    }

    IEnumerator FlameThrower()
    {
        while (gameObject.activeSelf)
        {
            hitTimer = 0f;
            effect.Play();
            yield return new WaitForSeconds(interval);

            hitTimer = 0f;
            effect.Stop();
            yield return new WaitForSeconds(interval);
        }
    }
}
