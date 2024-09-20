using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBarUI : MonoBehaviour,ITag
{
    [SerializeField] string detailTag;
    [SerializeField] Slider hpSlider;
    [SerializeField] Image hpBarColor;
    [SerializeField] TextMeshProUGUI TMPname;
    [SerializeField] Animator HpbarAniCTRL;
    [SerializeField] Color[] hpBarColors;
    int colorIndex = 0;
    int hpIndex = 0;
    int maxHp = 1;
    float[] hpColorChangeNum= new float[3];
    string bossName = "";

    private void OnEnable()
    {
        HpbarAniCTRL=GetComponent<Animator>();
        hpSlider = GetComponentInChildren<Slider>();
        TMPname = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void Init(int maxHp, float[] hpColorChangeNum, string name)
    {
        if(!HpbarAniCTRL)
            HpbarAniCTRL = GetComponent<Animator>();
        if(!hpSlider)
            hpSlider = GetComponentInChildren<Slider>();
        if(!TMPname)
            TMPname = GetComponentInChildren<TextMeshProUGUI>();
        if(hpColorChangeNum!=null)
            colorIndex = hpColorChangeNum.Length;
        hpIndex = 0;
        this.maxHp = maxHp;
        this.hpColorChangeNum = hpColorChangeNum;
        this.bossName = name;
        hpSlider.value = 1;
        ChangeHpBarColor(maxHp);
    }
    public void ActivateUI()
    {
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
        ChangeHpBarColor(curHp);
    }
    void ChangeHpBarColor(int hp)
    {
        if (hpColorChangeNum == null)
            return;

        foreach (Color color in hpBarColors)
        {
            if ((hpIndex + 1) >= hpColorChangeNum.Length) break;
            if (hp <= hpColorChangeNum[hpIndex] && hp > hpColorChangeNum[hpIndex + 1])
            {
                hpBarColor.color = color;
                hpIndex = 0;
                colorIndex = hpColorChangeNum.Length;
                return;
            }
            colorIndex--;
            hpIndex++;
        }

        hpBarColor.color = hpBarColors[hpBarColors.Length-1];
        colorIndex = hpBarColors.Length;
        hpIndex = 0;
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
