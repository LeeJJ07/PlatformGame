using System.Collections;
using UnityEngine;

public class AltarPortal : MonoBehaviour
{
    public static bool IsEnteringAltarPortal = false;

    [SerializeField] private Camera mainCam, portalCam;
    [SerializeField] private BoxCollider portalTrigger;
    [SerializeField] private Transform stagePortal;
    [SerializeField] private Animator coffinAnim, portalCamAnim;
    [SerializeField] private ParticleSystem portalTrailEffect, portalSparkleEffect;
    private CameraFollow cameraFollow;
    private bool isPlayerInPortalRange = false;

    void Start()
    {
        cameraFollow = GameObject.FindWithTag("MainCamera").GetComponent<CameraFollow>();
        portalTrigger.enabled = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (!isPlayerInPortalRange && other.gameObject.CompareTag("Player"))
        {
            isPlayerInPortalRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        isPlayerInPortalRange = false;
    }

    void Update()
    {
        if (isPlayerInPortalRange && portalTrigger.enabled && Input.GetButtonDown("Attack"))
        {
            portalTrigger.enabled = false;
            StartCoroutine(EnterPortal());
        }
    }
    
    public void TryOpenStagePortal(bool canOpenPortal)
    {
        if (canOpenPortal)
        {
            StartCoroutine(PlayPortalAnimation());
            portalTrigger.enabled = true;
        }
    }

    IEnumerator PlayPortalAnimation()
    {
        cameraFollow.SetFollowTarget(stagePortal);
        coffinAnim.Play(AnimationHash.COFFINDOOR_OPEN);

        yield return new WaitForSeconds(0.2f);

        portalTrailEffect.gameObject.SetActive(true);
        portalSparkleEffect.gameObject.SetActive(true);
        SoundManager.Instance.PlaySound("portal_idle", true, SoundType.BGM);

        portalTrailEffect.Play();
        portalSparkleEffect.Play();

        yield return new WaitForSeconds(2.3f);

        cameraFollow.SetFollowTarget();
    }
    
    IEnumerator EnterPortal()
    {
        mainCam.enabled = false;
        portalCam.enabled = IsEnteringAltarPortal = true;
        GameDirector.instance.PlayerControl.FixatePlayerRigidBody(true);
        GameDirector.instance.SetPlayerInventoryUI(false);

        yield return new WaitForSeconds(0.5f);
        portalCamAnim.Play(AnimationHash.PORTALCAM_ZOOM);

        yield return new WaitForSeconds(1.5f);

        portalCam.enabled = IsEnteringAltarPortal = false;
        SoundManager.Instance.StopLoopSound("portal_idle");
        GameDirector.instance.ShowLoadingScreen();
        StartCoroutine(GameDirector.instance.LoadNextStage());
    }
}
