using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    Collider col;
    [SerializeField] private GameObject coinEffect;

    private void Start()
    {
        col = GetComponent<Collider>();
        Invoke("CanPickUp", 0.5f);
    }
    void CanPickUp()
    {
        col.enabled = true;
    }
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

            SoundManager.Instance.PlaySound("Coin");
        }
    }
}
