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
        if (other.gameObject.CompareTag("Monster"))
        {
            EnemyControler enemyHP = other.gameObject.GetComponent<EnemyControler>();
            enemyHP.health -= slashDamage;
            Debug.Log("적에게 검기 명중");
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
