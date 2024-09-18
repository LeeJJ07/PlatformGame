using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemText : MonoBehaviour {
    TextMeshProUGUI textMP;

    public float textUpSpeed;
    public float alphaSpeed;
    public float destroyTime;

    [HideInInspector] public string itemText;

    private void Start() {
        textMP = GetComponent<TextMeshProUGUI>();
        textMP.text = itemText;

        Invoke("DestroyDamageText", destroyTime);
    }

    private void Update() {
        transform.Translate(new Vector3(0, textUpSpeed * Time.deltaTime, 0));
        textMP.color = new Color(1, 1, 1, Mathf.Lerp(textMP.color.a, 0, Time.deltaTime * alphaSpeed));
    }

    private void DestroyDamageText() {
        Destroy(gameObject);
    }
}
