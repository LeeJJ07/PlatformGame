using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpringPlatform : MonoBehaviour
{
    [SerializeField] private Animator springAnim;
    [SerializeField] private float springForce;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerControlManagerFix player = GameDirector.instance.PlayerControl;
            Rigidbody playerRb = player.GetComponent<Rigidbody>();

            playerRb.velocity = new Vector3(0, 0, 0);
            playerRb.AddForce(Vector3.up * springForce, ForceMode.Impulse);

            springAnim.Play(AnimationHash.SPRING, -1, 0f);

            SoundManager.Instance.PlaySound("Spring");
        }
    }
}
