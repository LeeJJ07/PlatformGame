using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblePlatform : MonoBehaviour
{
    [SerializeField] private Animator platformAnim;
    [SerializeField] private Rigidbody platformRb;
    private bool startCrumble = false;

    void OnTriggerEnter(Collider other)
    {
        if (!startCrumble && other.gameObject.CompareTag("Player"))
        {
            startCrumble = true;
            platformAnim.Play(AnimationHash.CRUMBLEPLATFORM_SHAKE);
        }
    }

    void DropPlatform()
    {
        platformRb.isKinematic = false;
        platformAnim.Play(AnimationHash.CRUMBLEPLATFORM_FALL);
    }

    void DisablePlatform()
    {
        gameObject.SetActive(false);
    }
}
