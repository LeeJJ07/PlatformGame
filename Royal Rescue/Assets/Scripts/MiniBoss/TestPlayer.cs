using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float power = 10f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "MiniBossBaseAttack":
                Debug.Log("오우 베이스!");
                break;
            case "MiniBossSkill1":
                break;
            case "MiniBossSkill2":
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Monster"))
            return;
        Debug.Log("ㅇ엥");
        Vector3 dir = (transform.position - other.transform.position).normalized;
        rb.AddForce(new Vector3(dir.x * power, 0f, 0f),ForceMode.Impulse);
    }
}
