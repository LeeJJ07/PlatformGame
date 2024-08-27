using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpBar : MonoBehaviour
{
    public UnityEngine.UI.Slider hpBar;
    public float maxHp;
    public float currentHp;
    public GameObject player;

    private void Start()
    {
        maxHp = player.GetComponent<PlayerControlManagerFix>().playerMaxHP;
        currentHp = player.GetComponent<PlayerControlManagerFix>().playerHP;
        hpBar.value = currentHp / maxHp;
    }
    void Update()
    {
        maxHp = player.GetComponent<PlayerControlManagerFix>().playerMaxHP;
        currentHp = player.GetComponent<PlayerControlManagerFix>().playerHP;
        hpBar.value = Mathf.Lerp(hpBar.value, currentHp / maxHp, Time.deltaTime * 10);
    }
}
