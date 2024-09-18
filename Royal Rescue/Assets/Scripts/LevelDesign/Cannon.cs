using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private float fireInterval;
    [SerializeField] private GameObject cannonBall;
    [SerializeField] private ParticleSystem explosionEffect;
    [SerializeField] private Transform cannon, spawnPoint;

    void OnEnable()
    {
        InvokeRepeating("FireCannonBall", 3f, fireInterval);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void FireCannonBall()
    {
        GameObject ball = Instantiate(cannonBall, spawnPoint.position, cannon.rotation);
        ball.transform.SetParent(transform);
        explosionEffect.Play();
    }
}
