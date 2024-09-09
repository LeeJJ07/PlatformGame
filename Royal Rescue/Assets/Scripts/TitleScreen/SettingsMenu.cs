using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : UIMenu
{
    private enum SettingType { FULLSCREEN, RESOLUTION, BGM, SFX };
    public delegate void OnSettingsExit();
    public OnSettingsExit OnSettingsExitDelegate { get; set; }


    void OnEnable()
    {
        SetMenuHighlight(menuIndex, false);
        base.Start();
    }

    void Update()
    {
        Navigate(KeyCode.DownArrow, KeyCode.UpArrow);
        UpdateSettings();
        CheckExitMenu();
    }

    private void UpdateSettings()
    {
        switch ((SettingType)menuIndex)
        {
            case SettingType.FULLSCREEN:
                break;
            
            case SettingType.RESOLUTION:
                break;
            
            case SettingType.BGM:
                break;
            
            case SettingType.SFX:
                break;

            default:
                break;
        }
    }

    private void CheckExitMenu()
    {
        if (pressedEscBtn)
        {
            OnSettingsExitDelegate?.Invoke();
            this.enabled = false;
        }
    }

}
