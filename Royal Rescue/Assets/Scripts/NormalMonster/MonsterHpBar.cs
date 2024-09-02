using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHpBar : MonoBehaviour
{
    Camera uiCamera;
    Canvas canvas;
    RectTransform rectParent;
    RectTransform rectHp;

    [HideInInspector] public Vector3 offset = Vector3.zero;
    [HideInInspector] public Transform targetTr;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset); // ������ ���� 3d��ǥ�� ��ũ����ǥ�� ��ȯ

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos); // ��ũ�� ��ǥ�� �ٽ� ü�¹� UI ĵ���� ��ǥ�� ��ȯ

        rectHp.localPosition = localPos; // ü�¹� ��ġ����
    }
}
