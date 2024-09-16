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
        textMP.text = "�ʿ� ���� \n: " + curCoin.ToString();

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
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset); // ������ ���� 3d��ǥ�� ��ũ����ǥ�� ��ȯ

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos); // ��ũ�� ��ǥ�� �ٽ� ü�¹� UI ĵ���� ��ǥ�� ��ȯ

        rectHp.localPosition = localPos; // ü�¹� ��ġ����

        if (curCoin > 0)
        {
            textMP.text = "�ʿ� ���� \n: " + curCoin.ToString();
            return;
        }
        isSuccess = true;
        textMP.text = "���� ���� ����!!";
        StartCoroutine(Deactive());
    }
    IEnumerator Deactive()
    {
        yield return new WaitForSeconds(1f);
        Destroy(textMP);
    }
}
