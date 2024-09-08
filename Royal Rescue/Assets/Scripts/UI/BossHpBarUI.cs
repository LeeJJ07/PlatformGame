using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBarUI : MonoBehaviour
{
    [SerializeField] string findSliderParent;
    [SerializeField] string findHpSlider;
    [SerializeField] string findNameTMP;
    GameObject[] UIobjs;
    Slider hpSlider;
    TextMeshProUGUI TMPname;
    int maxHp = 1;
    int curHp = 1;
    float[] hpColorChangeNum= new float[3];

    private void Start()
    {
        UIobjs = GameObject.FindGameObjectsWithTag("UI");
        ITag detailTag = null;
        foreach (GameObject obj in UIobjs)
        {
            detailTag = obj.GetComponent<ITag>();
            if (detailTag.CompareToTag(findSliderParent))
            {
                hpSlider = obj.GetComponentInChildren<Slider>();
                TMPname = obj.GetComponentInChildren<TextMeshProUGUI>();
            }
        }
    }
    public void init(int maxHp, int curHp, float[] hpColorChangeNum)
    {
        this.maxHp = maxHp;
        this.curHp = curHp;
        this.hpColorChangeNum = hpColorChangeNum;

        hpSlider.value = 1;
        TMPname.text = name;
        
    }
    public void changeHpValue(int value)
    {
        hpSlider.value = (float)(maxHp/value);
    }
}
