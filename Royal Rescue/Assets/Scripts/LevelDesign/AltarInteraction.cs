using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarInteraction : MonoBehaviour
{
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private GameObject gem;
    private AltarControl altarControl;
    private bool isPlayerInAltarRange = false;

    void Start()
    {
        altarControl = GameObject.FindWithTag("AltarControl").GetComponent<AltarControl>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isPlayerInAltarRange && other.gameObject.CompareTag("Player"))
        {
            isPlayerInAltarRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        isPlayerInAltarRange = false;
    }

    void Update()
    {
        if (isPlayerInAltarRange && Input.GetButtonDown("Attack") && altarControl.CanActivateAltar(resourceType))
        {
            gameObject.SetActive(false);
            gem.SetActive(true);
            GameDirector.instance.PlayerControl.inventory.UseGem(resourceType);
            // Play Sound
            altarControl.ActivateAltar();
        }
    }
}
