using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] private Rigidbody ballRb;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float horizontalForce;
    [SerializeField] private int damage;
    void Start()
    {
        SetBallVelocity();
        Destroy(gameObject, 10f);
    }

    void OnDisable()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject effect = Instantiate(hitEffect, transform.parent);
        effect.transform.position = collision.contacts[0].point;
        effect.GetComponent<ParticleSystem>().Play();

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerControlManagerFix player = GameDirector.instance.PlayerControl;

            player.SetPlayerVelocity(0, 0, 0);
            player.AddForceToPlayer(-ballRb.transform.right * horizontalForce, ForceMode.Impulse);
            player.HurtPlayer(damage);
        }
        Destroy(effect, 0.4f);
        Destroy(gameObject);
    }

    private void SetBallVelocity()
    {
        ballRb.velocity = -transform.right * moveSpeed;
    }
}
