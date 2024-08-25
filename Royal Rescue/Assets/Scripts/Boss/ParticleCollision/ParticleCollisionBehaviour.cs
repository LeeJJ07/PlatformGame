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
            //플레이어 데미지 전달 블럭
            Debug.Log("player HIT");
        }
    }
}
