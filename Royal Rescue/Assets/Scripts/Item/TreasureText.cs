using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreasureText : MonoBehaviour
{
    TextMeshPro textMP;
    public int curCoin;

    bool isSuccess = false;
    private void Start()
    {
        textMP = GetComponent<TextMeshPro>();
        textMP.text = "�ʿ� ���� \n: " + curCoin.ToString();
    }
    private void Update()
    {
        if (isSuccess)
        {
            return;
        }
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
