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
    }
    private void Update()
    {
        if (isSuccess)
            return;

        if (Camera.main == null || targetTr == null)
            return;
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);
        rectHp.localPosition = localPos;

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
