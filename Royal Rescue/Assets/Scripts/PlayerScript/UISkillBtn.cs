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
    [SerializeField] private float skillCollDown = 3.0f;


    public void Init()//�ش� ��ũ��Ʈ ���ư� �� ���� �ʱ�ȭ
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
        }
        else if (Input.GetButtonUp(skillName) && coolTimeRoutine == null)
        {
            if (skillName == "FireBallKey")
            {

                coolTimeRoutine = StartCoroutine(FbCoolTimeRoutine());
            }
        }
    }

    //�뽬 ��Ÿ�� �ڷ�ƾ
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
            this.imgFill.fillAmount = per;

            if (time <= 0)
            {
                Debug.Log("�뽬 ��");
                this.textCoolTime.gameObject.SetActive(false);
                break;
            }
            yield return null;
        }
        this.coolTimeRoutine = null;
    }

    //��ź ��Ÿ�� �ڷ�ƾ
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
                Debug.Log("��ų��");
                this.textCoolTime.gameObject.SetActive(false);
                break;
            }
            yield return null;
        }

        this.coolTimeRoutine = null;

    }


}

