using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBarUI : MonoBehaviour,ITag
{
    [SerializeField] string detailTag;
    [SerializeField] Slider hpSlider;
    [SerializeField] TextMeshProUGUI TMPname;
    int maxHp = 1;
    float[] hpColorChangeNum= new float[3];
    string bossName = "";

    private void OnEnable()
    {
        hpSlider = GetComponentInChildren<Slider>();
        TMPname = GetComponentInChildren<TextMeshProUGUI>();
        hpSlider.gameObject.SetActive(false);
        TMPname.gameObject.SetActive(false);
    }
    public void Init(int maxHp, float[] hpColorChangeNum, string name)
    {
        this.maxHp = maxHp;
        this.hpColorChangeNum = hpColorChangeNum;
        this.bossName = name;
    }
    public void ActivateUI()
    {
        hpSlider.gameObject.SetActive(true);
        TMPname.gameObject.SetActive(true);
        hpSlider.value = 1;
        TMPname.text = bossName;

    }
    public void DeActivateUI()
    {
        hpSlider.gameObject.SetActive(false);
        TMPname.gameObject.SetActive(false);
    }
    public void ChangeHpValue(int curHp)
    {
        hpSlider.value = (curHp/ (float)maxHp);
        Debug.Log($"value: {(curHp / (float)maxHp)}\n maxHp: {maxHp}\ncurHp: {curHp}");
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
