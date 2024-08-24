using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallControl : MonoBehaviour
{
    //Rigidbody rigidbody;
    public float throwForce = 10;
    public GameObject player;
    public bool isDirPlayer;
    public Vector3 ballDir;
    public int damage = 50; // 폭탄이 적에게 주는 데미지
    public GameObject explosionEffect; // 폭발 효과
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        //rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        //Vector3 dir = player.GetComponent<PlayerControlManagerFix>().isDirRight ? Vector3.right : Vector3.left;
        transform.position += throwForce * ballDir * Time.deltaTime;
        Destroy(this.gameObject, 2f);
        //포물선
        // rigidbody.AddForce(direction * throwForce, ForceMode.Impulse);
    }
    // Update is called once per frame
    // 폭탄이 발사될 때 호출하는 함수
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // 적에게 데미지를 입힘 (예시)
            //EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            //if (enemyHealth != null)
            //enemyHealth.TakeDamage(damage);
            EnemyControler enemyHP = other.gameObject.GetComponent<EnemyControler>();
            enemyHP.health -= damage;
            Debug.Log("적에게 파이어볼 명중");
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
                Destroy(explosion, 1.0f);
            }
            else
            {
                // 파티클 시스템이 없을 경우, 안전하게 일정 시간 후에 삭제
                Destroy(explosion, 3.0f);
            }
        }

        Destroy(gameObject);
    }
}
