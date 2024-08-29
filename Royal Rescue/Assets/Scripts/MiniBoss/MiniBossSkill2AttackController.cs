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

        //GameDirector.instance.PlayerControl.gameObject.SetActive(false);
        Debug.Log("플레이어 스킬 2 맞아따!");
    }
}
