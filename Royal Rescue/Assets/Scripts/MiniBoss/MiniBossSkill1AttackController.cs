using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossSkill1AttackController : MonoBehaviour
{
    [SerializeField] MiniBossAI miniBoss;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        GameDirector.instance.PlayerControl.HurtPlayer(miniBoss.GetSkill1Damage());
    }
}
