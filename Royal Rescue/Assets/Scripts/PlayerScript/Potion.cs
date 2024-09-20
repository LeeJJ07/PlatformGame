using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Potion : MonoBehaviour
{
    public UnityEngine.UI.Image itemImage; // �������� �̹���.
    private float coolTime;
    public int potionCount;//���� ���� �÷��̾�� �޾ƿ;���

    public TMP_Text textCoolTime;
    private PlayerControlManagerFix playerCntl;
    //private Coroutine coolTimeRoutine;
    public GameObject thisSkill;
    public Image imgFill;
    public int skillCollDown;
    private bool isDrinkingPotion = true;

    // �ʿ��� ������Ʈ.
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
        playerCntl = GameDirector.instance.PlayerControl;
        this.textCoolTime.gameObject.SetActive(false);

        Init();
    }
    private void Update()
    {
        if (Input.GetButtonDown("DrinkingPotion") && isDrinkingPotion)
        {
            if(potionCount > 0 && playerCntl.playerHP < playerCntl.playerMaxHP)
            {
                SoundManager.Instance.PlaySound("DrinkingPotion");
                isDrinkingPotion = false;
                if (playerCntl.playerHP + 50 <= playerCntl.playerMaxHP)
                    playerCntl.playerHP += 50;
                else
                    playerCntl.playerHP = playerCntl.playerMaxHP;
                StartCoroutine(PotionCoolTimeRoutine());
            }
            else if(playerCntl.playerHP == playerCntl.playerMaxHP)
            {
                Debug.Log("HP�� �̹� �ִ�ġ��");
            }
            else
            {
                Debug.Log("Not enough potion");
            }
        }
        text_Count.text = potionCount.ToString();
    }

    // ������ ȹ��



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
                isDrinkingPotion = true;
                Debug.Log("�໡�� ����");
                this.textCoolTime.gameObject.SetActive(false);
                break;
            }
            yield return null;
        }

    }
}
