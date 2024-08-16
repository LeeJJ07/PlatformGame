using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePlatform : MonoBehaviour
{
    [SerializeField] private GameObject hitEffect;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어의 체력을 ?만큼 감소.");

            //PlayerController player = GameDirector.instance.PlayerControl;
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            // rigidbody 도 public으로 빼서 접근할 수 있으면 좋을 것 같습니다
            Rigidbody playerRb = player.GetComponent<Rigidbody>();

            playerRb.velocity = new Vector3(0, 0, 0);
            playerRb.AddForce(Vector3.left * 15f, ForceMode.Impulse);
            playerRb.AddForce(Vector3.up * 10f, ForceMode.Impulse);

            GameObject effect = Instantiate(hitEffect, transform.parent);
            effect.transform.position = player.transform.position;

            effect.GetComponent<ParticleSystem>().Play();
            Destroy(effect, 0.4f);
        }
    }
}
