using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreasureText : MonoBehaviour
{
    Camera uiCamera;
    Canvas canvas;
    RectTransform rectParent;
    RectTransform rectHp;

    [HideInInspector] public Vector3 offset = Vector3.zero;
    [HideInInspector] public Transform targetTr;

    TextMeshPro textMP;
    public int curCoin;

    bool isSuccess = false;
    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();

        textMP = GetComponent<TextMeshPro>();
        textMP.text = "필요 코인 \n: " + curCoin.ToString();

        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (isSuccess)
        {
            return;
        }

        if (Camera.main == null || targetTr == null)
            return;
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset); // 몬스터의 월드 3d좌표를 스크린좌표로 변환

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos); // 스크린 좌표를 다시 체력바 UI 캔버스 좌표로 변환

        rectHp.localPosition = localPos; // 체력바 위치조정

        if (curCoin > 0)
        {
            textMP.text = "필요 코인 \n: " + curCoin.ToString();
            return;
        }
        isSuccess = true;
        textMP.text = "상자 열기 성공!!";
        StartCoroutine(Deactive());
    }
    IEnumerator Deactive()
    {
        yield return new WaitForSeconds(1f);
        Destroy(textMP);
    }
}
