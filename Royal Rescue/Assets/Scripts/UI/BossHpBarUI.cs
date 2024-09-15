using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBarUI : MonoBehaviour,ITag
{
    [Header("오브젝트 비활성화 하지 말 것")]
    [SerializeField] string detailTag;
    [SerializeField] Slider hpSlider;
    [SerializeField] TextMeshProUGUI TMPname;
    Animator HpbarAniCTRL;
    int maxHp = 1;
    float[] hpColorChangeNum= new float[3];
    string bossName = "";

    /*private void OnEnable()
    {
        HpbarAniCTRL=GetComponent<Animator>();
        hpSlider = GetComponentInChildren<Slider>();
        TMPname = GetComponentInChildren<TextMeshProUGUI>();
    }*/
    public void Init(int maxHp, float[] hpColorChangeNum, string name)
    {
        if(!HpbarAniCTRL)
            HpbarAniCTRL = GetComponent<Animator>();
        if(!hpSlider)
            hpSlider = GetComponentInChildren<Slider>();
        if(!TMPname)
            TMPname = GetComponentInChildren<TextMeshProUGUI>();

        this.maxHp = maxHp;
        this.hpColorChangeNum = hpColorChangeNum;
        this.bossName = name;
        hpSlider.value = 1;
    }
    public void ActivateUI()
    {
        hpSlider.value = 1;
        TMPname.text = bossName;
        HpbarAniCTRL.SetBool("isActivate", true);
        HpbarAniCTRL.SetBool("isDeActivate", false);
    }
    public void DeActivateUI()
    {
        HpbarAniCTRL.SetBool("isActivate", false);
        HpbarAniCTRL.SetBool("isDeActivate", true);
    }
    public void ChangeHpValue(int curHp)
    {
        hpSlider.value = (curHp/ (float)maxHp);
    }

    public string GetTag()
    {
        return detailTag;
    }

    public bool CompareToTag(string detailTag)
    {
        return this.detailTag == detailTag;
    }
}
