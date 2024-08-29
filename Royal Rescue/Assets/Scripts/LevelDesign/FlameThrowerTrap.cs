using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerTrap : MonoBehaviour
{
    [SerializeField] private float interval;
    [SerializeField] private ParticleSystem effect;
    void OnEnable()
    {
        StartCoroutine(FlameThrower());
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator FlameThrower()
    {
        while (gameObject.activeSelf)
        {
            effect.Play();
            yield return new WaitForSeconds(interval);

            effect.Stop();
            yield return new WaitForSeconds(interval);
        }
    }
}
