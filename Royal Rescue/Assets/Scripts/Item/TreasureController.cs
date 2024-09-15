using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TreasureController : MonoBehaviour
{
    [Header("상자 종류")]
    [SerializeField] private bool isRuby;
    [SerializeField] private bool isDiamond;
    [SerializeField] private bool isJade;
    [SerializeField] private bool isPotion;


    [SerializeField] int needRandomCoin = 0;
    [SerializeField] int curCoin = 0;
    [SerializeField] int minCoinNum = 5;
    [SerializeField] int maxCoinNum = 11;

    GameObject treasureText;
    private Canvas uiCanvas;
    public GameObject treasureTextPrefab;
    public GameObject treasureCoinPrefab;
    public GameObject treasureParticlePrefab;
    public GameObject[] itemPrefabs; 

    bool successInputCoin = false;
    private void Start()
    {
        needRandomCoin = Random.Range(minCoinNum, maxCoinNum);
        curCoin = needRandomCoin;
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
        if (!other.CompareTag("Player"))
            return;
        if (treasureText == null) {
            uiCanvas = GameObject.Find("InGame Canvas").GetComponent<Canvas>();
            Vector3 nVec = new Vector3(0, 1.7f, 0);
            var screenPos = Camera.main.WorldToScreenPoint(transform.position + nVec);
            var localPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(uiCanvas.GetComponent<RectTransform>(), screenPos, uiCanvas.worldCamera, out localPos);

            treasureText = Instantiate(treasureTextPrefab, uiCanvas.transform) as GameObject;
            treasureText.GetComponent<TreasureText>().curCoin = curCoin;
            treasureText.transform.SetParent(uiCanvas.transform, true);
            treasureText.transform.localPosition = localPos;
        }
        else
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
            Instantiate(treasureCoinPrefab, transform);
            yield return new WaitForSeconds(0.3f);
        }
        Instantiate(treasureParticlePrefab, transform.position, Quaternion.identity);
        if (isPotion) {
            RandomItemCreate();
            Debug.Log("생겼다.");
        } else if (isRuby)
            RubyCreate();
        else if (isDiamond)
            DiamondCreate();
        else if (isJade)
            JadeCreate();

        Destroy(gameObject);
    }
    void RandomItemCreate()
    {
        int idx = Random.Range(0, 4);
        GameObject item = Instantiate(itemPrefabs[idx],transform.position, Quaternion.identity);
        item.GetComponent<Rigidbody>().AddForce(Vector3.up * 1000);
        item.GetComponent<Collider>().enabled = false;
    }
    void RubyCreate() {
        int idx = 4;
        GameObject item = Instantiate(itemPrefabs[idx], transform.position, Quaternion.identity);
        item.GetComponent<Rigidbody>().AddForce(Vector3.up * 1000);
        item.GetComponent<Collider>().enabled = false;
    }
    void DiamondCreate() {
        int idx = 5;
        GameObject item = Instantiate(itemPrefabs[idx], transform.position, Quaternion.identity);
        item.GetComponent<Rigidbody>().AddForce(Vector3.up * 1000);
        item.GetComponent<Collider>().enabled = false;
    }
    void JadeCreate() {
        int idx = 6;
        GameObject item = Instantiate(itemPrefabs[idx], transform.position, Quaternion.identity);
        item.GetComponent<Rigidbody>().AddForce(Vector3.up * 1000);
        item.GetComponent<Collider>().enabled = false;
    }
}
