using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePlatform : MonoBehaviour
{
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private float horizontalForce, verticalForce;
    [SerializeField] private int damage;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerControlManagerFix player = GameDirector.instance.PlayerControl;

            // rigidbody 도 public으로 빼서 접근할 수 있으면 좋을 것 같습니다
            Rigidbody playerRb = player.GetComponent<Rigidbody>();

            playerRb.velocity = new Vector3(0, 0, 0);
            playerRb.AddForce(Vector3.left * horizontalForce, ForceMode.Impulse);
            playerRb.AddForce(Vector3.up * verticalForce, ForceMode.Impulse);

            GameObject effect = Instantiate(hitEffect, transform.parent);
            effect.transform.position = player.transform.position;

            effect.GetComponent<ParticleSystem>().Play();
            Destroy(effect, 0.4f);
            
            GameDirector.instance.PlayerControl.HurtPlayer(damage);
        }
    }
}
