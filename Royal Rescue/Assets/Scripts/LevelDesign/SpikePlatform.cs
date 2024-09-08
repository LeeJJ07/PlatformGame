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

            player.SetPlayerVelocity(0, 0, 0);
            player.AddForceToPlayer(Vector3.left * horizontalForce, ForceMode.Impulse);
            player.AddForceToPlayer(Vector3.up * verticalForce, ForceMode.Impulse);

            GameObject effect = Instantiate(hitEffect, transform.parent);
            effect.transform.position = player.transform.position;
            effect.GetComponent<ParticleSystem>().Play();
            Destroy(effect, 0.4f);
            
            player.HurtPlayer(damage);
        }
    }
}
