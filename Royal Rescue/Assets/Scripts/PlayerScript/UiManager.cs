using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    //[SerializeField] Movement playerScript;
    [SerializeField] UnityEngine.UI.Slider healthBar;
    [SerializeField] TextMeshProUGUI healthText;
    /*
    private void Update()
    {
        healthBar.value = playerScript.currentHealth;
        healthBar.maxValue = playerScript.maxHealth;
        healthText.text = $"{healthBar.value} / {healthBar.maxValue}";
    }*/
}
