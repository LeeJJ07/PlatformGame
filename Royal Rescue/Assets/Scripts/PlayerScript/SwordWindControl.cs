using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWindControl : MonoBehaviour
{
    public float throwForce = 20;
    public bool isFireball = false;
    public Vector3 ballDir;
    public int slashDamage = 35;
    public GameObject explosionEffect;
    public GameObject target;
    private void Update()
    {
        transform.position += throwForce * ballDir * Time.deltaTime;
        Destroy(this.gameObject, 0.3f);
    }
    private void OnCollisionEnter(Collision other)
    {
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
