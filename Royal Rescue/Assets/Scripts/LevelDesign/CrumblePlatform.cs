using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblePlatform : MonoBehaviour
{
    [SerializeField] private Animator platformAnim;
    [SerializeField] private Rigidbody platformRb;
    [SerializeField] private BoxCollider platformCollider;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private float respawnDelay;

    private bool startCrumble = false;
    private Vector3 initialPos;
    private bool hasBeenInitialized = false;

    void OnEnable()
    {
        if (!hasBeenInitialized)
        {
            hasBeenInitialized = true;
            initialPos = platformRb.position;
        }
        platformRb.isKinematic = true;
        platformRb.position = initialPos;
        startCrumble = false;
        meshRenderer.enabled = platformCollider.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!startCrumble && other.gameObject.CompareTag("Player"))
        {
            startCrumble = true;
            SoundManager.Instance.PlaySound("Crumble");
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
        meshRenderer.enabled = platformCollider.enabled = false;
        platformRb.isKinematic = true;
        StartCoroutine(ResetPlatform());
    }

    IEnumerator ResetPlatform()
    {
        yield return new WaitForSeconds(respawnDelay);

        platformRb.position = initialPos;
        startCrumble = false;
        meshRenderer.enabled = platformCollider.enabled = true;
        platformAnim.Play(AnimationHash.CRUMBLEPLATFORM_RESPAWN);
    }
}
