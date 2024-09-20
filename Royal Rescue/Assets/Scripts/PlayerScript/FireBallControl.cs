using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallControl : MonoBehaviour
{
    Rigidbody rigidbody;
    public float throwForce = 10.0f;
    public bool isFireball = false;
    public Vector3 ballDir;

    public int bombDamage = 50; // 폭탄이 적에게 주는 데미지
    public GameObject explosionEffect; // 폭발 효과
    public GameObject target;
    PlayerControlManagerFix player;


    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlaySound("BombThrowing");
        rigidbody = GetComponent<Rigidbody >();
        rigidbody.AddForce((ballDir + Vector3.up * 1.5f) * throwForce, ForceMode.Impulse);//포물선
    }
    private void Update()
    {
        Destroy(this.gameObject, 2f);
        
    }
    // Update is called once per frame
    // 폭탄이 발사될 때 호출하는 함수
    private void OnCollisionEnter(Collision other)
    {
        SoundManager.Instance.PlaySound("BombExplosion");
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyControler enemyHP = other.gameObject.GetComponent<EnemyControler>();
            enemyHP.health -= bombDamage;
            Debug.Log("적에게 파이어볼 명중");
        }
        if (other.gameObject.CompareTag("Player"))
        {
            return;
        }
        else
        {
            Destroy(this.gameObject);
        }
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);

            ParticleSystem ps = explosion.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(explosion, 1.0f);
            }
            else
            {
                Destroy(explosion, 3.0f);
            }
        }
        Destroy(gameObject);
    }
}
