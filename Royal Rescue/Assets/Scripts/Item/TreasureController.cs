using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TreasureController : MonoBehaviour {
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
    private void Start() {
        needRandomCoin = Random.Range(minCoinNum, maxCoinNum);
        curCoin = needRandomCoin;

        SetText();
    }
    private void Update() {
        if (treasureText == null || !treasureText.activeSelf || successInputCoin)
            return;
        if (!GameDirector.instance.PlayerControl.InputCoinKeyDown())
            return;
        if (curCoin > GameDirector.instance.PlayerControl.GetCoin()) {
            StartCoroutine(CantInputCoin());
            return;
        }
        successInputCoin = true;
        GameDirector.instance.PlayerControl.SetCoin(curCoin);
        StartCoroutine(InputCoinActive());
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player") || treasureText == null)
            return;

        treasureText.SetActive(true);
    }
    private void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Player") || treasureText == null)
            return;
        treasureText.SetActive(false);
    }
    IEnumerator CantInputCoin() {
        treasureText.GetComponent<TextMeshPro>().color = Color.red;
        yield return new WaitForSeconds(1f);
        treasureText.GetComponent<TextMeshPro>().color = Color.white;
    }
    IEnumerator InputCoinActive() {
        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(true);
        treasureText.GetComponent<TreasureText>().curCoin = 0;
        treasureText.GetComponent<TextMeshPro>().color = Color.yellow;
        yield return new WaitForSeconds(1f);
        Destroy(treasureText);
        while (curCoin > 0) {
            curCoin--;
            Instantiate(treasureCoinPrefab, transform);
            yield return new WaitForSeconds(0.3f);
        }

        SoundManager.Instance.PlaySound("OpenTreasure");

        Instantiate(treasureParticlePrefab, transform.position, Quaternion.identity);
        if (isPotion) {
            RandomItemCreate();
        } else if (isRuby)
            RubyCreate();
        else if (isDiamond)
            DiamondCreate();
        else if (isJade)
            JadeCreate();

        Destroy(gameObject);
        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(false);
    }

    private void SetText() 
    {
        uiCanvas = GameObject.Find("InGame Canvas").GetComponent<Canvas>();

        treasureText = Instantiate<GameObject>(treasureTextPrefab, uiCanvas.transform);

        var _treasureText = treasureText.GetComponent<TreasureText>();
        _treasureText.targetTr = this.gameObject.transform;
        _treasureText.offset = new Vector3(0, 1.7f, 0);
        _treasureText.curCoin = curCoin;
        _treasureText.gameObject.SetActive(false);
    }
    void RandomItemCreate() {
        int idx = Random.Range(0, 4);
        GameObject item = Instantiate(itemPrefabs[idx], transform.position, Quaternion.identity);
        item.GetComponent<Rigidbody>().AddForce(Vector3.up * 500);
        item.GetComponent<Collider>().enabled = false;
    }
    void RubyCreate() {
        int idx = 4;
        GameObject item = Instantiate(itemPrefabs[idx], transform.position, Quaternion.identity);
        item.GetComponent<Rigidbody>().AddForce(Vector3.up * 500);
        item.GetComponent<Collider>().enabled = false;
    }
    void DiamondCreate() {
        int idx = 5;
        GameObject item = Instantiate(itemPrefabs[idx], transform.position, Quaternion.identity);
        item.GetComponent<Rigidbody>().AddForce(Vector3.up * 500);
        item.GetComponent<Collider>().enabled = false;
    }
    void JadeCreate() {
        int idx = 6;
        GameObject item = Instantiate(itemPrefabs[idx], transform.position, Quaternion.identity);
        item.GetComponent<Rigidbody>().AddForce(Vector3.up * 500);
        item.GetComponent<Collider>().enabled = false;
    }
}
