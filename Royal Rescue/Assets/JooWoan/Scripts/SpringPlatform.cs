using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPlatform : MonoBehaviour
{
    [SerializeField] private Animator springAnim;
    [SerializeField] private float springForce;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            Rigidbody playerRb = player.GetComponent<Rigidbody>();

            playerRb.velocity = new Vector3(0, 0, 0);
            playerRb.AddForce(Vector3.up * springForce, ForceMode.Impulse);

            springAnim.Play("Spring", -1, 0f);
        }
    }
}
