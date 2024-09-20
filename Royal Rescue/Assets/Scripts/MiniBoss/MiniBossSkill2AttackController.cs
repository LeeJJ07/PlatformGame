using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossSkill2AttackController : MonoBehaviour
{
    [SerializeField] MiniBossAI miniBoss;
    private void OnParticleCollision(GameObject other)
    {
        if (!other.CompareTag("Player"))
            return;

        GameDirector.instance.PlayerControl.HurtPlayer(miniBoss.GetSkill2Damage());
    }
}
