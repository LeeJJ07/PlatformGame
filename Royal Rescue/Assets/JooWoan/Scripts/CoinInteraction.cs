using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinInteraction : MonoBehaviour
{
    [SerializeField] private GameObject coinEffect;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("동전 획득!");
            gameObject.SetActive(false);

            GameObject effect = Instantiate(coinEffect, transform.parent);
            effect.transform.position = other.transform.position;
            effect.GetComponent<ParticleSystem>().Play();
            Destroy(effect, 0.9f);
        }
    }
}
