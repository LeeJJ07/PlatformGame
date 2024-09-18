using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AltarInteraction : MonoBehaviour
{
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private GameObject gem;
    private AltarControl altarControl;
    private bool isPlayerInAltarRange = false;

    private TextMeshProUGUI altarText;

    void Start()
    {
        altarControl = GameObject.FindWithTag("AltarControl").GetComponent<AltarControl>();

        //연산량이 많을 수도 있음..
        altarText = GameObject.Find("InGame Canvas").GetComponent<Canvas>().GetComponentInChildren<TextMeshProUGUI>(true);
        altarText.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isPlayerInAltarRange && other.gameObject.CompareTag("Player"))
        {
            isPlayerInAltarRange = true;

            string resourceName = "";
            switch(resourceType)
            {
                case ResourceType.RUBY:
                    resourceName = "루비";
                    break;
                case ResourceType.DIAMOND:
                    resourceName = "다이아몬드";
                    break;
                case ResourceType.JADE:
                    resourceName = "옥";
                    break;
            }
            altarText.text = "[Z] : " + resourceName + " 올리기";
            altarText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        isPlayerInAltarRange = false;

        altarText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInAltarRange && Input.GetButtonDown("Attack") && altarControl.CanActivateAltar(resourceType))
        {
            gameObject.SetActive(false);
            gem.SetActive(true);
            altarText.gameObject.SetActive(false);
            GameDirector.instance.PlayerControl.inventory.UseGem(resourceType);
            SoundManager.Instance.PlaySound("InputGem");
            altarControl.ActivateAltar();
        }
    }
}
