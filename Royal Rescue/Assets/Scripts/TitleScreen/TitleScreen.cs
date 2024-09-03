using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    private enum ScreenState { INTRO, TITLE, MAIN, HELP, SETTINGS };
    private enum MenuState { START, HOWTOPLAY, SETTINGS, EXIT };
    private const float FADEAMOUNT = 0.0001f;
    
    [SerializeField] private Image fadeCover;
    [SerializeField] private Animator cameraAnim;
    [SerializeField] private TextMeshProUGUI titleText, startText;
    [SerializeField] private PostProcessVolume ppVolume;
    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private float screenFadeSpeed, textFadeSpeed, menuFadeSpeed, blurSpeed;
    [SerializeField] private float initialFocusDistance;
    [SerializeField] private ScreenState previousScreenState, currentScreenState;
    private DepthOfField dof;

    void Awake()
    {
        fadeCover.gameObject.SetActive(true);
        titleText.gameObject.SetActive(false);
        startText.gameObject.SetActive(false);
        mainMenu.alpha = 0f;

        ppVolume.profile.TryGetSettings(out dof);
        previousScreenState = currentScreenState = ScreenState.INTRO;
    }

    void Start()
    {
        StartCoroutine(StartIntro());
    }

    void Update()
    {
        UpdateScreen();
    }
    
    private void UpdateScreen()
    {
        switch (currentScreenState)
        {
            case ScreenState.INTRO:
                break;

            case ScreenState.TITLE:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    previousScreenState = currentScreenState;
                    currentScreenState = ScreenState.MAIN;
                    StartCoroutine(ShowMainMenu());
                }
                break;

            case ScreenState.MAIN:
                break;
            
            case ScreenState.HELP:
                break;
        
            case ScreenState.SETTINGS:
                break;
            
            default:
                break;
        }
    }

    private IEnumerator StartIntro()
    {
        yield return new WaitForSeconds(0.5f);
        cameraAnim.Play(AnimationHash.TITLESCREENCAM_INTRO);

        yield return Fade(fadeCover, true);

        yield return new WaitForSeconds(0.5f);
        titleText.gameObject.SetActive(true);
        yield return Fade(titleText, true);

        yield return new WaitForSeconds(0.5f);
        startText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        currentScreenState = ScreenState.TITLE;
    }

    private IEnumerator Fade(Image targetImage, bool isFadeIn)
    {
        float initialAlpha = 1f;
        float multiplier = -1f;

        if (!isFadeIn)
        {
            initialAlpha = 0f;
            multiplier = 1f;
        }
        Color tempColor = targetImage.color;
        tempColor.a = initialAlpha;
        targetImage.color = tempColor;

        while (true)
        {
            tempColor = targetImage.color;
            tempColor.a += FADEAMOUNT * multiplier * screenFadeSpeed;
            targetImage.color = tempColor;

            if (isFadeIn && targetImage.color.a <= 0f) break;
            else if (!isFadeIn && targetImage.color.a >= 1f) break;

            yield return null;
        }
    }

    private IEnumerator Fade(TextMeshProUGUI targetText, bool isFadeIn)
    {
        targetText.alpha = 0f;
        float multiplier = 1f;
        
        if (!isFadeIn)
        {
            targetText.alpha = 1f;
            multiplier = -1f;
        }
        while (true)
        {
            targetText.alpha += FADEAMOUNT * multiplier * textFadeSpeed;

            if (isFadeIn && targetText.alpha >= 1f) break;
            else if (!isFadeIn && targetText.alpha <= 0f) break;

            yield return null;
        }
    }

    private IEnumerator Fade(CanvasGroup group, bool isFadeIn)
    {
        group.alpha = 0f;
        float multiplier = 1f;
        
        if (!isFadeIn)
        {
            group.alpha = 1f;
            multiplier = -1f;
        }
        while (true)
        {
            group.alpha += FADEAMOUNT * multiplier * menuFadeSpeed;

            if (isFadeIn && group.alpha >= 1f) break;
            else if (!isFadeIn && group.alpha <= 0f) break;

            yield return null;
        }
    }

    private IEnumerator ShowMainMenu()
    {
        startText.gameObject.SetActive(false);
        yield return Fade(titleText, false);
        yield return BlurScreen();
        yield return Fade(mainMenu, true);
        
        currentScreenState = ScreenState.MAIN;
    }

    private IEnumerator BlurScreen()
    {
        if (!dof) yield break;
        dof.focusDistance.value = initialFocusDistance;

        while (true)
        {
            dof.focusDistance.value -= 0.001f * blurSpeed;
            if (dof.focusDistance.value <= 0.1f)
            {
                dof.focusDistance.value = 0.1f;
                break;
            }
            yield return null;
        }
    }

}
