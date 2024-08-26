using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    Rigidbody rb;
    public int health = 500;
    public int enemyAtk = 500;
    public GameObject player;
    private void Update()
    {
        rb = GetComponent<Rigidbody>();
        TakeDamage();
    }
    public void TakeDamage()
    {
        if (health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        // �� ������Ʈ�� �ı�
        Destroy(gameObject);
        
        player.GetComponent<PlayerControlManagerFix>().isAttackEnhance = true;
    }
    void OnCollisionEnter(Collision collision)
    {
       
    }
}
