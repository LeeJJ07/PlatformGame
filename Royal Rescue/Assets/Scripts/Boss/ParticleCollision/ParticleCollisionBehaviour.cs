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
    /// ��ƼŬ �ý��� �ʱ� ����
    /// </summary>
    /// <param name="triggerCollider">�ν��� �ݶ��̴� ����</param>
    /// <param name="isContinuous">�������� ���������� �ٰ��� ����</param>
    /// <param name="damageTime">���ʿ� �������� �ٰ��� ����</param>
    /// <param name="damage">������ ����</param>
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
            //�÷��̾� ������ ���� ��
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
