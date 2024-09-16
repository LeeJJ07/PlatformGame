using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBossHpBarBehavior : MonoBehaviour
{
    [SerializeField] string BossName;
    int hp = 0;
    BossHpBarUI hpbarUi;
    Monster monster;
    bool isActive = false;
    void OnEnable()
    {
        monster=GetComponent<Monster>();
        GameObject[] uiObjs = GameObject.FindGameObjectsWithTag("UI");
        foreach (GameObject obj in uiObjs)
        {
            if (obj.GetComponent<ITag>().CompareToTag("BossHpUI"))
                hpbarUi = obj.GetComponent<BossHpBarUI>();
        }
        
    }

    void Update()
    {
        
        if (hpbarUi&& !isActive)
        {
            hpbarUi.Init(monster.GetMaxHp(), null, BossName);
            hpbarUi.ActivateUI();
            isActive = true;
        }
        hp = monster.GetCurHp();
        if (hp <= 0)
        {
            hpbarUi.DeActivateUI();
        }
        hpbarUi.ChangeHpValue(hp);
    }
    private void OnDisable()
    {
        if(isActive)
        {
            hpbarUi.DeActivateUI();
            isActive = false;
        }
    }
}
