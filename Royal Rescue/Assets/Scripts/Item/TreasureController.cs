using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TreasureController : MonoBehaviour
{
    [SerializeField] int needRandomCoin = 0;
    [SerializeField] int curCoin = 0;
    [SerializeField] int minCoinNum = 5;
    [SerializeField] int maxCoinNum = 11;

    public Vector3 hpBarOffset = new Vector3(0, -0.4f, 0);

    GameObject treasureText;
    protected Canvas uiCanvas;
    public GameObject treasureTextPrefab;
    public GameObject treasureCoinPrefab;
    public GameObject treasureParticlePrefab;
    public GameObject[] itemPrefabs; 

    bool successInputCoin = false;
    private void Start()
    {
        needRandomCoin = Random.Range(minCoinNum, maxCoinNum);
        curCoin = needRandomCoin;

        uiCanvas = GameObject.Find("InGame Canvas").GetComponent<Canvas>();
        Vector3 nVec = new Vector3(0, 1.7f, 0);
        var screenPos = Camera.main.WorldToScreenPoint(transform.position + nVec); // 몬스터의 월드 3d좌표를 스크린좌표로 변환
        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(uiCanvas.GetComponent<RectTransform>(), screenPos, uiCanvas.worldCamera, out localPos);

        treasureText = Instantiate(treasureTextPrefab) as GameObject;
        treasureText.GetComponent<TreasureText>().curCoin = curCoin;
        treasureText.transform.SetParent(uiCanvas.transform, false);
        treasureText.transform.localPosition = localPos;
        treasureText.SetActive(false);
    }
    private void Update()
    {
        if (treasureText == null || !treasureText.activeSelf || successInputCoin)
            return;
        if (!GameDirector.instance.PlayerControl.InputCoinKeyDown())
            return;
        if (curCoin > GameDirector.instance.PlayerControl.GetCoin())
        {
            StartCoroutine(CantInputCoin());
            return;
        }
        successInputCoin = true;
        GameDirector.instance.PlayerControl.SetCoin(curCoin);
        StartCoroutine(InputCoinActive());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || treasureText == null)
            return;
        treasureText.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || treasureText == null)
            return;
        treasureText.SetActive(false);
    }
    IEnumerator CantInputCoin()
    {
        treasureText.GetComponent<TextMeshPro>().color = Color.red;
        yield return new WaitForSeconds(1f);
        treasureText.GetComponent<TextMeshPro>().color = Color.white;
    }
    IEnumerator InputCoinActive()
    {
        treasureText.GetComponent<TreasureText>().curCoin = 0;
        treasureText.GetComponent<TextMeshPro>().color = Color.yellow;
        yield return new WaitForSeconds(1f);
        Destroy(treasureText);
        while (curCoin > 0)
        {
            curCoin--;
            Instantiate(treasureCoinPrefab);
            yield return new WaitForSeconds(0.3f);
        }
        Instantiate(treasureParticlePrefab);
        RandomItemCreate();
        Destroy(gameObject);
    }
    void RandomItemCreate()
    {
        int idx = Random.Range(0, itemPrefabs.Length);
        GameObject item = Instantiate(itemPrefabs[idx]);
        item.GetComponent<Rigidbody>().AddForce(Vector3.up * 1000);
        item.GetComponent<Collider>().enabled = false;
    }
}
