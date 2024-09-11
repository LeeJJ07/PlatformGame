using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveWall : MonoBehaviour
{
    [SerializeField] private GameObject gem;
    [SerializeField] private GameObject particle;

    // Update is called once per frame
    void Update()
    {
        if (gem.activeSelf == false)
        {
            gameObject.SetActive(false);
            GameObject effect = Instantiate(particle, transform.parent);
            effect.transform.position = transform.position;
            effect.GetComponent<ParticleSystem>().Play();
            Destroy(effect, 0.9f);
        }
    }
}
