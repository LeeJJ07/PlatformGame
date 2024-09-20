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
    public GameObject thisSkill;
    public string skillName;
    public Image imgFill;
    private int skillCnt;
    private Coroutine coolTimeRoutine;
    private float skillCollDown = 10.0f;
    private float dashCollDown = 1.0f;


    public void Init()//해당 스크립트 돌아갈 때 상태 초기화
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
        if (Input.GetButtonDown(skillName) && coolTimeRoutine == null)
        {
            if (skillName == "Dash")
            {
                coolTimeRoutine = StartCoroutine(DashCoolTimeRoutine());
            }
        }
        else if (Input.GetButtonUp(skillName) && coolTimeRoutine == null)
        {
            if (skillName == "FireBallKey" /*&& GameDirector.instance.PlayerControl.skillCount > 0*/)
            {
                coolTimeRoutine = StartCoroutine(FbCoolTimeRoutine());
            }
        }
    }

    //대쉬 쿨타임 코루틴
    private IEnumerator DashCoolTimeRoutine()
    {
        
        coolTime = dashCollDown;
        Debug.Log(textCoolTime);
        this.textCoolTime.gameObject.SetActive(true);
        var time = this.coolTime;

        while (true)
        {
            time -= Time.deltaTime;
            this.textCoolTime.text = time.ToString("F1");

            var per = time / this.coolTime;
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

    //폭탄 쿨타임 코루틴
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

