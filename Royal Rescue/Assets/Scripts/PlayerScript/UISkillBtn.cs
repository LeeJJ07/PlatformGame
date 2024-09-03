using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UISkillBtn : MonoBehaviour
{
    private float coolTime;

    public TMP_Text textCoolTime;
    public GameObject player;
    private Coroutine coolTimeRoutine;
    public GameObject thisSkill;
    public string skillName;
    public Image imgFill;
    public int skillCollDown;


    public void Init()
    {
        this.textCoolTime.gameObject.SetActive(false);
        this.imgFill.fillAmount = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        this.textCoolTime.gameObject.SetActive(false);
        
        Init();
    }
    void Update()
    {
        //fireBallCnt = player.GetComponent<PlayerControlManagerFix>().skillCount;

        if (Input.GetButtonDown(skillName) && coolTimeRoutine == null)
        {
            if (skillName == "Dash")
            {
                coolTimeRoutine = StartCoroutine(DashCoolTimeRoutine());
            }
            else if (skillName == "FireBallKey")
            {
                
                coolTimeRoutine = StartCoroutine(FbCoolTimeRoutine());
            }
        }
        //text_Count.text = fireBallCnt.ToString();
    }

    private IEnumerator DashCoolTimeRoutine()
    {
        
        coolTime = skillCollDown;
        Debug.Log(textCoolTime);
        this.textCoolTime.gameObject.SetActive(true);
        var time = this.coolTime;

        while (true)
        {
            time -= Time.deltaTime;
            this.textCoolTime.text = time.ToString("F1");

            var per = time / this.coolTime;
            //Debug.Log(per);
            this.imgFill.fillAmount = per;

            if (time <= 0)
            {
                Debug.Log("대쉬 온");
                this.textCoolTime.gameObject.SetActive(false);
                break;
            }
            yield return null;
        }
        this.coolTimeRoutine = null;
    }
    private IEnumerator FbCoolTimeRoutine()
    {
        coolTime = skillCollDown;
        Debug.Log(textCoolTime);
        this.textCoolTime.gameObject.SetActive(true);
        var time = this.coolTime;

        while (true)
        {
            time -= Time.deltaTime;
            this.textCoolTime.text = time.ToString("F1");

            var per = time / this.coolTime;
            //Debug.Log(per);
            this.imgFill.fillAmount = per;

            if (time <= 0)
            {
                Debug.Log("스킬온");
                this.textCoolTime.gameObject.SetActive(false);
                break;
            }
            yield return null;
        }

        this.coolTimeRoutine = null;

    }


}

