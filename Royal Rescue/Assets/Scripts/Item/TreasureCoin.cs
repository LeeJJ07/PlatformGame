using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreasureCoin : MonoBehaviour
{
    MeshRenderer mRenderer;

    public float coinUpSpeed;
    public float alphaSpeed;
    public float destroyTime;

    private Color coinColor;
    [SerializeField] private float rotSpeed = 160f;
    void Start()
    {
        SoundManager.Instance.PlaySound("InputCoin");
        mRenderer = GetComponent<MeshRenderer>();
        coinColor = mRenderer.material.GetColor("_Color");

        Invoke("DestroyCoin", destroyTime);
    }

    void Update()
    {
        transform.Translate(new Vector3(0, coinUpSpeed * Time.deltaTime, 0));

        transform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime, 0));

        float newAlpha = Mathf.Lerp(coinColor.a, 0, alphaSpeed * Time.deltaTime);
        coinColor.a = newAlpha;
        mRenderer.material.SetColor("_Color", coinColor);
    }
    private void DestroyCoin()
    {
        Destroy(gameObject);
    }
}
