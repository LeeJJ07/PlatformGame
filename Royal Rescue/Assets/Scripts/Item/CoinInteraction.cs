using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinIntecration : MonoBehaviour
{
    Collider col;
    [SerializeField] private GameObject coinEffect;
    private ItemObject itemObject;

    private void Start() {
        itemObject = GetComponent<ItemObject>();
        col = GetComponent<Collider>();
    }
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            itemObject.OnInteractCoin();
            gameObject.SetActive(false);

            GameObject effect = Instantiate(coinEffect, transform.parent);
            effect.transform.position = other.transform.position;
            effect.GetComponent<ParticleSystem>().Play();
            Destroy(effect, 0.9f);

            SoundManager.Instance.PlaySound("Coin");
        }
    }
}
