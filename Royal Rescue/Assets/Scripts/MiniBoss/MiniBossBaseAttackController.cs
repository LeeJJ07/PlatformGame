using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        Debug.Log("�÷��̾� �⺻���� �¾Ƶ�!");
    }
}
