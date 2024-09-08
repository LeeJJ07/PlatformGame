using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public enum ScreenState { INTRO, TITLE, MAIN, HELP, SETTINGS };
public class TitleScreen : MonoBehaviour
{
    private const float FADEAMOUNT = 0.0001f;
    
    [SerializeField] private TitleMenu titleMenuControl;
    [SerializeField] private Image fadeCover;
    [SerializeField] private Animator cameraAnim;
     [SerializeField] private TextMeshProUGUI titleText, startText;
    [SerializeField] private PostProcessVolume ppVolume;
    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private float screenFadeInSpeed, screenFadeOutSpeed, textFadeSpeed, menuFadeSpeed, blurSpeed;
    [SerializeField] private float initialFocusDistance;

    [SerializeField] private ScreenState currentScreenState;
    public ScreenState CurrentScreenState => currentScreenState;

    private DepthOfField dof;

    void OnEnable()
    {
        SetPlayerEnabled(false);
        Init();
        StartCoroutine(StartIntro());
    }

    void OnDisable()
    {
        SetPlayerEnabled(true);
    }

    void Update()
    {
        UpdateScreen();
    }
    
    private void UpdateScreen()
    {
        switch (currentScreenState)
        {
            case ScreenState.TITLE:
                if (UIMenu.pressedConfirmBtn)
                {
                    SetScreenState(ScreenState.MAIN);
                    StartCoroutine(ShowMainMenu());
                }
                break;
            
            case ScreenState.HELP:
                break;
        
            case ScreenState.SETTINGS:
                break;
            
            default:
                break;
        }
    }

    private void Init()
    {
        ppVolume.profile.TryGetSettings(out dof);
        fadeCover.gameObject.SetActive(true);
        titleText.gameObject.SetActive(false);
        startText.gameObject.SetActive(false);
        titleMenuControl.enabled = false;
        mainMenu.alpha = 0f;
        SetScreenState(ScreenState.INTRO);
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
        SetScreenState(ScreenState.TITLE);
    }

    private IEnumerator Fade(Image targetImage, bool isFadeIn)
    {
        float initialAlpha = 1f;
        float multiplier = -1f;
        float fadeSpeed = isFadeIn ? screenFadeInSpeed : screenFadeOutSpeed;

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
            tempColor.a += FADEAMOUNT * multiplier * fadeSpeed;
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

    public void SetScreenState(ScreenState state)
    {
        currentScreenState = state;
    }

    private IEnumerator ShowMainMenu()
    {
        startText.gameObject.SetActive(false);
        yield return Fade(titleText, false);
        yield return BlurScreen();
        yield return Fade(mainMenu, true);
        
        SetScreenState(ScreenState.MAIN);
        titleMenuControl.enabled = true;
    }

    private IEnumerator HideMainMenu(bool isCoverScreen = false)
    {
        titleMenuControl.enabled = false;

        yield return Fade(mainMenu, false);

        if (isCoverScreen)
            yield return StartCoroutine(Fade(fadeCover, false));
    }

    public IEnumerator StartGame()
    {
        yield return HideMainMenu(true);

        GameDirector.instance.ShowLoadingScreen();
        StartCoroutine(GameDirector.instance.LoadNextStage());
    }

    private void SetPlayerEnabled(bool state)
    {
        if (!GameDirector.instance) return;
        GameDirector.instance.PlayerControl.gameObject.SetActive(state);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
