using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlameThrowerTrap : MonoBehaviour
{
    [SerializeField] private float interval;
    [SerializeField] private float hitInterval;
    [SerializeField] private int damage;
    [SerializeField] private ParticleSystem effect;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private bool enableSfx = false;
    private const float ACTIVATE_DELAY = 3f;
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
            GameObject hitEf = Instantiate(hitEffect, transform);
            hitEf.transform.position = GameDirector.instance.PlayerControl.transform.position;
            hitEf.GetComponent<ParticleSystem>().Play();
            Destroy(hitEf, 0.4f);
            
            GameDirector.instance.PlayerControl.HurtPlayer(damage);
        }
        else if (hitTimer >= hitInterval)
            hitTimer = 0f;

        hitTimer += Time.deltaTime;
    }

    IEnumerator FlameThrower()
    {
        yield return new WaitForSeconds(ACTIVATE_DELAY);

        while (gameObject.activeSelf)
        {
            var coll = effect.collision;

            hitTimer = 0f;
            effect.Play();
            coll.enabled = true;
            if (enableSfx) SoundManager.Instance.PlaySound("flamethrower");
            yield return new WaitForSeconds(interval);

            hitTimer = 0f;
            effect.Stop();
            coll.enabled = false;
            
            yield return new WaitForSeconds(interval);
        }
    }
}
