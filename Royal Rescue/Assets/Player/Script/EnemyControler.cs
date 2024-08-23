using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    public int health = 500;
    public int enemyAtk = 500;
    public GameObject player;
    private void Update()
    {
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
    }
    void OnCollisionEnter(Collision collision)
    {
       
    }
}
