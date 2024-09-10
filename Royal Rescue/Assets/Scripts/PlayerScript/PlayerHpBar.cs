using System.Collections;
using System.Collections.Generic;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using static CheckHp;
using static System.Net.Mime.MediaTypeNames;

public class PlayerHpBar : MonoBehaviour
{
    public UnityEngine.UI.Slider hpBar;
    public float maxHp;
    public float currentHp;
    public UnityEngine.UI.Text PlayerHpText;
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
        

        if (currentHp <= 0)
        {
            currentHp = 0;
            PlayerHpText.text = string.Format("D E A D");
        }
        else
            PlayerHpText.text = string.Format("{0}  /  {1}", currentHp, maxHp);
    }
}
