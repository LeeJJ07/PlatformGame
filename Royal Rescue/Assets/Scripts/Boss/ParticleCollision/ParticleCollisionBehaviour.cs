using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionBehaviour : MonoBehaviour,ITag
{
    [Header("Detail Tag"), SerializeField]
    string detailTag;
    [SerializeField]int damage = 0;
    [SerializeField] float damageTime = 0;
    ParticleSystem ParticleSystem;
    Collider triggerCollider;
    
    [SerializeField]float span = 0;
    bool isContinuous = false;
    /// <summary>
    /// 파티클 시스템 초기 설정
    /// </summary>
    /// <param name="triggerCollider">인식할 콜라이더 설정</param>
    /// <param name="isContinuous">데미지를 지속적으로 줄건지 설정</param>
    /// <param name="damageTime">몇초에 데미지를 줄건지 설정</param>
    /// <param name="damage">데미지 설정</param>
    public void init(Collider triggerCollider,bool isContinuous,float damageTime ,int damage)
    {
        Debug.Log("particle init");
        ParticleSystem = GetComponent<ParticleSystem>();
        this.triggerCollider = triggerCollider;
        ParticleSystem.trigger.AddCollider(triggerCollider);
        this.isContinuous = isContinuous;
        this.damageTime = damageTime;
        this.damage = damage;
    }
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
        if(isContinuous)
        {
            span += Time.deltaTime;
            if(span>damageTime)
            {
                triggerCollider.GetComponent<PlayerControlManagerFix>().HurtPlayer(damage);
                span = 0;
            }
        }
        else
        {
            triggerCollider.GetComponent<PlayerControlManagerFix>().HurtPlayer(damage);
        }
    }
}
