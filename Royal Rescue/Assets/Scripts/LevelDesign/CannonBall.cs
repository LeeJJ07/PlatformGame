using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] private Rigidbody ballRb;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private float moveSpeed;
[SerializeField] private float horizontalForce;
    void Start()
    {
        SetBallVelocity();
        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject effect = Instantiate(hitEffect, transform.parent);
        effect.transform.position = collision.transform.position;
        effect.GetComponent<ParticleSystem>().Play();

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어의 체력을 ?만큼 감소.");

            PlayerControlManagerFix player = GameDirector.instance.PlayerControl;
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            playerRb.AddForce(-ballRb.transform.right * horizontalForce, ForceMode.Impulse);
        }
        Destroy(effect, 0.4f);
        Destroy(gameObject);
    }

    private void SetBallVelocity()
    {
        ballRb.velocity = -transform.right * moveSpeed;
    }
}
