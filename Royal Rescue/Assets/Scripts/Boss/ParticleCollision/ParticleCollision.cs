using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Player"))
        {
            //�÷��̾� ������ ���� �ڵ� �ۼ� ��
            Debug.Log("player HIT");
        }
    }
}
