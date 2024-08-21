using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarInteraction : MonoBehaviour
{
    [SerializeField] private GemType gemType;
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("상호작용");
        }
    }
}
