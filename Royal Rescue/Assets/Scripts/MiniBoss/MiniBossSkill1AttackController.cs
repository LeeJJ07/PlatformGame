using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossSkill1AttackController : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        Debug.Log("�÷��̾� ��ų 1 �¾Ƶ�!");
    }
}
