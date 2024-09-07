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
        textMP.text = "필요 코인 \n: " + curCoin.ToString();
    }
    private void Update()
    {
        if (isSuccess)
        {
            return;
        }
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
