using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    [SerializeField] MiniBossAI miniBoss;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        GameDirector.instance.PlayerControl.HurtPlayer(miniBoss.GetBaseAttackDamage());
        Debug.Log("�÷��̾� �⺻���� "+ miniBoss.GetBaseAttackDamage());
    }
}
