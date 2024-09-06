using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    TextMeshPro textMP;

    public float textUpSpeed;
    public float alphaSpeed;
    public float destroyTime;

    public int damage;

    [HideInInspector] public Vector3 offset = Vector3.zero;
    [HideInInspector] public Transform targetTr;

    private void Start()
    {
        textMP = GetComponent<TextMeshPro>();
        textMP.text = damage.ToString();

        Invoke("DestroyDamageText", destroyTime);
    }

    private void Update()
    {
        transform.Translate(new Vector3(0, textUpSpeed * Time.deltaTime, 0));
        textMP.color = new Color(1, 1, 1, Mathf.Lerp(textMP.color.a, 0, Time.deltaTime*alphaSpeed));
    }
    
    private void DestroyDamageText()
    {
        Destroy(gameObject);
    }
}
