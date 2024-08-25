using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Player"))
        {
            //플레이어 데미지 전달 코드 작석 블럭
            Debug.Log("player HIT");
        }
    }
}
