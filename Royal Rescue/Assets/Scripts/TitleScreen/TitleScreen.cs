using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    private enum ScreenState { INTRO, TITLE, MAIN };
    [SerializeField] private TitleMenu titleMenuControl;
    [SerializeField] private Animator cameraAnim;
    [SerializeField] private TextMeshProUGUI titleText, startText;
    [SerializeField] private PostProcessVolume ppVolume;
    [SerializeField] private Image screenCover;
    [SerializeField] private float initialFocusDistance;
    
    private DepthOfField dof;
    private ScreenState currentScreenState;

    public Image ScreenCover => screenCover;
    public float screenFadeInSpeed, screenFadeOutSpeed, textFadeSpeed, menuFadeSpeed, blurSpeed;

    void OnEnable()
    {
        currentScreenState = ScreenState.INTRO;
        GameDirector.instance.SetCursorVisibility(true);

        Init();
        GameDirector.instance.SetPlayerUI(false);
        GameDirector.instance.PlayerControl.SetPlayerEnabled(false);
        StartCoroutine(StartIntro());
    }

    void OnDisable()
    {
        if (GameDirector.instance)
        {
            GameDirector.instance.PlayerControl.SetPlayerEnabled(true);
            GameDirector.instance.PlayerControl.ToggleCursor(false);
        }
    }
    
    void Update()
    {
        if ((currentScreenState == ScreenState.TITLE) && UIMenu.pressedConfirmBtn)
        {
            currentScreenState = ScreenState.MAIN;
            StartCoroutine(HideTitle());
        }
    }
    
    private void Init()
    {
        ppVolume.profile.TryGetSettings(out dof);
        screenCover.gameObject.SetActive(true);
        titleText.gameObject.SetActive(false);
        startText.gameObject.SetActive(false);
    }

    private IEnumerator StartIntro()
    {
        yield return new WaitForSeconds(0.5f);
        cameraAnim.Play(AnimationHash.TITLESCREENCAM_INTRO);

        yield return FadeInOut.Fade(screenCover, true, screenFadeInSpeed, screenFadeOutSpeed);

        yield return new WaitForSeconds(0.5f);
        titleText.gameObject.SetActive(true);

        yield return FadeInOut.Fade(titleText, true, textFadeSpeed);

        yield return new WaitForSeconds(0.5f);
        startText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        currentScreenState = ScreenState.TITLE;
    }

    private IEnumerator HideTitle()
    {
        startText.gameObject.SetActive(false);
        yield return FadeInOut.Fade(titleText, false, textFadeSpeed);
        yield return BlurScreen();

        StartCoroutine(titleMenuControl.ShowMainMenu());
        titleMenuControl.enabled = true;
    }

    private IEnumerator BlurScreen()
    {
        if (!dof) yield break;
        dof.focusDistance.value = initialFocusDistance;

        while (true)
        {
            dof.focusDistance.value -= blurSpeed * Time.deltaTime;
            if (dof.focusDistance.value <= 0.1f)
            {
                dof.focusDistance.value = 0.1f;
                break;
            }
            yield return null;
        }
    }
}
