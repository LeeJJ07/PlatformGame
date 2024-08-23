using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarControl : MonoBehaviour
{
    public static int redGem, whiteGem, greenGem = 0;

    [SerializeField] private Transform stagePortal;
    [SerializeField] private Animator coffinAnim;
    [SerializeField] private ParticleSystem portalTrailEffect, portalSparkleEffect;

    private CameraFollow cameraFollow;
    private int activatedAltars = 0;
    private const int TOTALALTARS = 3;

    [SerializeField] private bool test; /////

    void Start()
    {
        cameraFollow = GameObject.FindWithTag("MainCamera").GetComponent<CameraFollow>();
    }

    public bool CanActivateAltar(GemType gemType)
    {
        switch (gemType)
        {
            case GemType.RED:
                return redGem > 0;

            case GemType.WHITE:
                return whiteGem > 0;                

            case GemType.GREEN:
                return greenGem > 0;
            
            default:
                return false;
        }
    }

    void Update()
    {
        if (test)
        {
            test = false;
            StartCoroutine(PlayPortalAnimation());
        }
    }

    public void ActivateAltar()
    {
        ++activatedAltars;
        TryOpenStagePortal();
    }

    private void TryOpenStagePortal()
    {
        if (activatedAltars == TOTALALTARS)
        {
            Debug.Log("다음 스테이지로의 포탈이 열렸습니다.");
            StartCoroutine(PlayPortalAnimation());
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
}

