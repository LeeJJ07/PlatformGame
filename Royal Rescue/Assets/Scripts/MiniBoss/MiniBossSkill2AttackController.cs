using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossSkill2AttackController : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("dafsdfdasf");
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("플레이어 스킬 2 맞아따!");
    }

    
}
