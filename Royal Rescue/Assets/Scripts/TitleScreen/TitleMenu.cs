using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TitleMenu : UIMenu
{
    private enum MenuState { START, HELP, SETTINGS, EXIT };
    [SerializeField] private TitleScreen titleScreen;
    [SerializeField] private PromptMenu exitPromptControl;
    [SerializeField] private GameObject exitPrompt;

    void Awake()
    {
        exitPromptControl.enabled = false;
        exitPrompt.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected void Update()
    {
        Navigate(KeyCode.DownArrow, KeyCode.UpArrow);
        SelectMenu();
    }

    protected void SelectMenu()
    {
        if (pressedConfirmBtn)
        {
            switch ((MenuState)menuIndex)
            {
                case MenuState.START:
                    StartCoroutine(titleScreen.StartGame());
                    break;

                case MenuState.HELP:
                    break;

                case MenuState.SETTINGS:
                    StartCoroutine(titleScreen.ShowSettingsMenu());
                    break;

                case MenuState.EXIT:
                    ShowExitPrompt();
                    break;
                
                default:
                    break;
            }
        }
    }

    private void ShowExitPrompt()
    {
        this.enabled = false;
        exitPromptControl.OnConfirmDelegate = titleScreen.QuitGame;
        exitPromptControl.OnCancelDelegate = EnableTitleMenu;
        exitPrompt.SetActive(true);
        exitPromptControl.enabled = true;
    }

    private void EnableTitleMenu()
    {
        exitPrompt.SetActive(false);
        this.enabled = true;
    }
}