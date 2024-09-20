using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Potion : MonoBehaviour
{
    public UnityEngine.UI.Image itemImage;
    private float coolTime;

    public TMP_Text textCoolTime;
    private PlayerControlManagerFix playerCntl;
    //private Coroutine coolTimeRoutine;
    public GameObject thisSkill;
    public Image imgFill;
    public int skillCollDown;
    private bool isDrinkingPotion = true;

    [SerializeField]
    private UnityEngine.UI.Text text_Count;
    [SerializeField]
    private GameObject go_CountImage;

    private Canvas uiCanvas;
    public GameObject increaseHealthTextPrefab;
    public void Init()
    {
        this.textCoolTime.gameObject.SetActive(false);
        this.imgFill.fillAmount = 0;
    }
    void Start()
    {
        playerCntl = GameDirector.instance.PlayerControl;
        this.textCoolTime.gameObject.SetActive(false);

        uiCanvas = GameObject.Find("InGame Canvas").GetComponent<Canvas>();
        Init();
    }
    private void Update()
    {
        if (Input.GetButtonDown("DrinkingPotion") && isDrinkingPotion)
        {
            if(playerCntl.inventory.healPotionCount > 0 && playerCntl.playerHP < playerCntl.playerMaxHP)
            {
                SoundManager.Instance.PlaySound("DrinkingPotion");
                isDrinkingPotion = false;
                HealingText(playerCntl.playerMaxHP - playerCntl.playerHP < 50 ? playerCntl.playerMaxHP - playerCntl.playerHP : 50);

                if (playerCntl.playerHP + 50 <= playerCntl.playerMaxHP)
                    playerCntl.playerHP += 50;
                else
                    playerCntl.playerHP = playerCntl.playerMaxHP;

                playerCntl.inventory.healPotionCount--;
                StartCoroutine(PotionCoolTimeRoutine());
            }
        }
        text_Count.text = playerCntl.inventory.healPotionCount.ToString();
    }


    private IEnumerator PotionCoolTimeRoutine()
    {
        
        coolTime = skillCollDown;
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
                isDrinkingPotion = true;
                this.textCoolTime.gameObject.SetActive(false);
                break;
            }
            yield return null;
        }
    }
    private void HealingText(int healAmount) {
        if (Camera.main != null) {
            Vector3 nVec = new Vector3(0, 1.5f, 0);
            var screenPos = Camera.main.WorldToScreenPoint(playerCntl.gameObject.transform.position + nVec);
            var localPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(uiCanvas.GetComponent<RectTransform>(), screenPos, uiCanvas.worldCamera, out localPos); // 스크린 좌표를 다시 체력바 UI 캔버스 좌표로 변환

            GameObject healUI = Instantiate(increaseHealthTextPrefab) as GameObject;
            healUI.GetComponent<HealText>().healAmount = healAmount;
            healUI.transform.SetParent(uiCanvas.transform, false);
            healUI.transform.localPosition = localPos;
            healUI.GetComponent<HealText>().colorR = 0f;
            healUI.GetComponent<HealText>().colorB = 0f;
        }
    }

    public void ResetPotion()
    {
        StopCoroutine(PotionCoolTimeRoutine());
        coolTime = 0;
        imgFill.fillAmount = 0;
        textCoolTime.gameObject.SetActive(false);
        isDrinkingPotion = true;
    }
    
}
