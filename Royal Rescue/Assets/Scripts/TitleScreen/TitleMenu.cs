using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenu : UIMenu
{
    private enum MenuState { START, HELP, SETTINGS, EXIT };
    [SerializeField] private TitleScreen titleScreen;
    [SerializeField] private PromptMenu exitPromptControl;
    [SerializeField] private SettingsMenu settingsControl;
    [SerializeField] private HelpMenu helpControl;
    [SerializeField] private CanvasGroup mainMenuGroup, settingsMenuGroup, helpMenuGroup;
    [SerializeField] private GameObject exitPrompt, settingsMenu, helpMenu;

    void Awake()
    {
        this.enabled = false;
        exitPromptControl.enabled = false;
        helpControl.enabled = false;
        settingsControl.enabled = false;

        mainMenuGroup.alpha = 0f;
        helpMenuGroup.alpha = 0f;
        settingsMenuGroup.alpha = 0f;

        exitPrompt.SetActive(false);
        helpMenu.SetActive(false);
        settingsMenu.gameObject.SetActive(false);
    }
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        Navigate(KeyCode.DownArrow, KeyCode.UpArrow);
        SelectMenu();
    }

    private void SelectMenu()
    {
        if (pressedConfirmBtn)
        {
            switch ((MenuState)menuIndex)
            {
                case MenuState.START:
                    StartCoroutine(StartGame());
                    break;

                case MenuState.HELP:
                    StartCoroutine(ShowHelpMenu());
                    break;

                case MenuState.SETTINGS:
                    StartCoroutine(ShowSettingsMenu());
                    break;

                case MenuState.EXIT:
                    ShowExitPrompt();
                    break;
                
                default:
                    break;
            }
            this.enabled = false;
        }
    }
    
    private IEnumerator HideMainMenu(bool isCoverScreen = false)
    {
        yield return FadeInOut.Fade(mainMenuGroup, false, titleScreen.menuFadeSpeed);

        if (isCoverScreen)
            yield return StartCoroutine(FadeInOut.Fade(titleScreen.ScreenCover, false, titleScreen.screenFadeInSpeed, titleScreen.screenFadeOutSpeed));
    }

    public IEnumerator ShowMainMenu()
    {
        yield return FadeInOut.Fade(mainMenuGroup, true, titleScreen.menuFadeSpeed);
    }

    private IEnumerator ShowSettingsMenu()
    {
        yield return HideMainMenu();

        settingsMenu.SetActive(true);
        yield return FadeInOut.Fade(settingsMenuGroup, true, titleScreen.menuFadeSpeed);
        
        settingsControl.OnSettingsExitDelegate = EnableTitleMenu;
        settingsControl.enabled = true;
    }

    private IEnumerator ShowHelpMenu()
    {
        yield return HideMainMenu();

        helpMenu.SetActive(true);
        yield return FadeInOut.Fade(helpMenuGroup, true, titleScreen.menuFadeSpeed);

        helpControl.OnHelpCancelDelegate = EnableTitleMenu;
        helpControl.enabled = true;
    }

    private void ShowExitPrompt()
    {
        exitPromptControl.OnConfirmDelegate = QuitGame;
        exitPromptControl.OnCancelDelegate = EnableTitleMenu;

        exitPrompt.SetActive(true);
        exitPromptControl.enabled = true;
    }

    private void EnableTitleMenu()
    {
        helpMenu.SetActive(false);
        settingsMenu.SetActive(false);
        exitPrompt.SetActive(false);

        if (mainMenuGroup.alpha != 1f)
            StartCoroutine(ShowMainMenu());
        
        this.enabled = true;
    }

    private IEnumerator StartGame()
    {
        yield return HideMainMenu(true);

        GameDirector.instance.ShowLoadingScreen();
        StartCoroutine(GameDirector.instance.LoadNextStage());
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}