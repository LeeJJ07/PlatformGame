using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionBehaviour : MonoBehaviour,ITag
{
    [Header("Detail Tag"), SerializeField]
    string detailTag;

    public bool CompareToTag(string detailTag)
    {
        return this.detailTag == detailTag;
    }

    public string GetTag()
    {
        return detailTag;
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Player"))
        {
            //�÷��̾� ������ ���� ��
            Debug.Log("player HIT");
        }
    }
}
