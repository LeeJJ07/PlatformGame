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
            Debug.Log("�÷��̾��� ü���� ?��ŭ ����.");

            //PlayerController player = GameDirector.instance.PlayerControl;
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            // rigidbody �� public���� ���� ������ �� ������ ���� �� �����ϴ�
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
