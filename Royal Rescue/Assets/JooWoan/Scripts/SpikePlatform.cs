using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePlatform : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitEffect;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�÷��̾��� ü���� ?��ŭ ����.");

            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            Rigidbody playerRb = player.GetComponent<Rigidbody>();

            playerRb.velocity = new Vector3(0, 0, 0);
            playerRb.AddForce(Vector3.left * 15f, ForceMode.Impulse);
            playerRb.AddForce(Vector3.up * 10f, ForceMode.Impulse);

            hitEffect.Play();
        }
    }
}
