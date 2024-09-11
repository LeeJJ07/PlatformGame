using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    Collider col;
    [SerializeField] GameObject effectPrefab;

    private void Start()
    {
        col = GetComponent<Collider>();
        Invoke("CanPickUp", 0.5f);
    }
    void CanPickUp()
    {
        col.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        Instantiate(effectPrefab, transform);
        Destroy(gameObject);
    }
}
