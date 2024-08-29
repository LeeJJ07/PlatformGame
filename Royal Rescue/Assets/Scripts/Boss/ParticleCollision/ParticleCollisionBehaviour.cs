using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionBehaviour : MonoBehaviour,ITag
{
    [Header("Detail Tag"), SerializeField]
    string detailTag;
    [SerializeField]int damage = 0;
    public bool CompareToTag(string detailTag)
    {
        return this.detailTag == detailTag;
    }
    public string GetTag()
    {
        return detailTag;
    }
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("collision Particle!");
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerControlManagerFix>().HurtPlayer(damage);
            //플레이어 데미지 전달 블럭
            Debug.Log($"{name}: player HIT");
        }
 
    }
    private void OnParticleTrigger()
    {
        Debug.Log("Particle trigger!");
    }
}
