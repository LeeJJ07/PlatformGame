using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWindControl : MonoBehaviour
{
    //Rigidbody rigidbody;
    public float throwForce = 20;
    public bool isFireball = false;
    public Vector3 ballDir;
    public int slashDamage = 35;
    public GameObject explosionEffect;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        //rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        transform.position += throwForce * ballDir * Time.deltaTime;
        Destroy(this.gameObject, 0.3f);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyControler enemyHP = other.gameObject.GetComponent<EnemyControler>();
            enemyHP.health -= slashDamage;
            Debug.Log("적에게 검기 명중");
        }
        if (other.gameObject.CompareTag("Player"))
        {
            // 플레이어와 충돌할 경우 아무 작업도 하지 않음
            return;
        }
        else
        {
            Destroy(this.gameObject);
        }
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);

            // 파티클의 재생 시간을 계산한 후, 해당 시간이 지나면 파티클 오브젝트를 삭제
            ParticleSystem ps = explosion.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(explosion, 1f);
            }
            else
            {
                Destroy(explosion, 3.0f);
            }
        }

        Destroy(gameObject);
    }
}
