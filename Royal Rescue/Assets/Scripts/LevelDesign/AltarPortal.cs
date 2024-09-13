using System.Collections;
using UnityEngine;

public class AltarPortal : MonoBehaviour
{
    [SerializeField] private Camera mainCam, portalCam;
    [SerializeField] private BoxCollider portalTrigger;
    [SerializeField] private Transform stagePortal;
    [SerializeField] private Animator coffinAnim, portalCamAnim;
    [SerializeField] private ParticleSystem portalTrailEffect, portalSparkleEffect;
    private CameraFollow cameraFollow;

    private bool isPlayerInPortalRange = false;
    // [SerializeField] private bool test_openPortal; /////


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
        if (isPlayerInPortalRange && Input.GetButtonDown("Attack"))
        {
            portalTrigger.enabled = false;
            StartCoroutine(EnterPortal());
        }
    }
    
    public void TryOpenStagePortal(bool canOpenPortal)
    {
        if (canOpenPortal)
        {
            Debug.Log("다음 스테이지로의 포탈이 열렸습니다.");
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

        portalTrailEffect.Play();
        portalSparkleEffect.Play();

        yield return new WaitForSeconds(2.3f);

        cameraFollow.SetFollowTarget();
    }
    
    IEnumerator EnterPortal()
    {
        mainCam.enabled = false;
        portalCam.enabled = true;

        yield return new WaitForSeconds(0.5f);
        portalCamAnim.Play(AnimationHash.PORTALCAM_ZOOM);

        yield return new WaitForSeconds(1.5f);

        portalCam.enabled = false;
        GameDirector.instance.ShowLoadingScreen();
        StartCoroutine(GameDirector.instance.LoadNextStage());
    }
}
