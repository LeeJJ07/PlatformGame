using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Potion : MonoBehaviour
{
    public UnityEngine.UI.Image itemImage; // 아이템의 이미지.
    private float coolTime;
    public int potionCount;

    public TMP_Text textCoolTime;
    public GameObject player;
    //private Coroutine coolTimeRoutine;
    public GameObject thisSkill;
    public Image imgFill;
    public int skillCollDown;

    // 필요한 컴포넌트.
    [SerializeField]
    private UnityEngine.UI.Text text_Count;
    [SerializeField]
    private GameObject go_CountImage;
    public void Init()
    {
        this.textCoolTime.gameObject.SetActive(false);
        this.imgFill.fillAmount = 0;
    }
    void Start()
    {
        this.textCoolTime.gameObject.SetActive(false);

        Init();
    }
    private void Update()
    {
        if (Input.GetButtonDown("DrinkingPotion"))
        {
            if(potionCount > 0 && player.GetComponent<PlayerControlManagerFix>().playerHP < player.GetComponent<PlayerControlManagerFix>().playerMaxHP)
            {
                if (player.GetComponent<PlayerControlManagerFix>().playerHP + 50 <= player.GetComponent<PlayerControlManagerFix>().playerMaxHP)
                    player.GetComponent<PlayerControlManagerFix>().playerHP += 50;
                else
                    player.GetComponent<PlayerControlManagerFix>().playerHP = player.GetComponent<PlayerControlManagerFix>().playerMaxHP;
                StartCoroutine(PotionCoolTimeRoutine());
            }
            else if(player.GetComponent<PlayerControlManagerFix>().playerHP == player.GetComponent<PlayerControlManagerFix>().playerMaxHP)
            {
                Debug.Log("HP가 이미 최대치임");
            }
            else
            {
                Debug.Log("Not enough potion");
            }
        }
        text_Count.text = potionCount.ToString();
    }

    // 아이템 획득



    private IEnumerator PotionCoolTimeRoutine()
    {
        potionCount -= 1;
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
                Debug.Log("약빨기 가능");
                this.textCoolTime.gameObject.SetActive(false);
                break;
            }
            yield return null;
        }

        //this.coolTimeRoutine = null;

    }
}
