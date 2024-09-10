using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsMenu : UIMenu
{
    private enum SettingType { FULLSCREEN, RESOLUTION, BGM, SFX };
    private const float VOLUME_STEP = 0.05f;
    private const float SLIDER_MIN = 0.0001f;
    private const float SLIDER_MAX = 1f;
    private const int MIN_RESOLUTION = 4;
    private const int MAX_RESOLUTION = 8;

    public delegate void OnSettingsExit();
    public OnSettingsExit OnSettingsExitDelegate { get; set; }

    [SerializeField] private List<Slider> volumeSliders;
    [SerializeField] private List<TextMeshProUGUI> volumeTexts;
    [SerializeField] private List<string> fullScreenTextlist;
    [SerializeField] private TextMeshProUGUI fullScreenModeText, resolutionText;

    private int resolutionIndex = -1;
    private bool isFullScreen = false;


    protected override void Start()
    {
        volumeSliders[(int)SoundType.BGM].value = ConvertVolumeToValue(SoundManager.Instance.CurrentBGMVolume);
        volumeSliders[(int)SoundType.EFFECT].value = ConvertVolumeToValue(SoundManager.Instance.CurrentSFXVolume);
        base.Start();

        FindMatchingResolution();
        isFullScreen = Screen.fullScreen;
    }

    void OnDisable()
    {
        // When application is about to quit, this.enabled will be true.
        if (this.enabled) return;

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
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    isFullScreen = !isFullScreen;
                    fullScreenModeText.text = fullScreenTextlist[Convert.ToInt32(isFullScreen)];
                    Screen.fullScreen = isFullScreen;
                }
                break;
            
            case SettingType.RESOLUTION:
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    --resolutionIndex;
                    SetResolution();
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    ++resolutionIndex;
                    SetResolution();
                }
                break;

            case SettingType.BGM:
            case SettingType.SFX:
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    volumeSliders[menuIndex - 2].value -= VOLUME_STEP;
                
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                    volumeSliders[menuIndex - 2].value += VOLUME_STEP;
                break;

            default:
                break;
        }
    }

    private void FindMatchingResolution()
    {
        for (int i = 4; i <= 8; i++)
        {
            if (Screen.currentResolution.width == 240 * i)
            {
                resolutionIndex = i;
                break;
            }
        }
        if (resolutionIndex < 0)
            resolutionIndex = MAX_RESOLUTION;
        SetResolution();
        fullScreenModeText.text = fullScreenTextlist[Convert.ToInt32(isFullScreen)];
    }

    private void SetResolution()
    {
        resolutionIndex = Mathf.Clamp(resolutionIndex, MIN_RESOLUTION, MAX_RESOLUTION);
        (int, int) currentResolution = (240 * resolutionIndex, 135 * resolutionIndex);
        resolutionText.text = $"{currentResolution.Item1} x {currentResolution.Item2}";
        Screen.SetResolution(currentResolution.Item1, currentResolution.Item2, Screen.fullScreen);
    }

    private void CheckExitMenu()
    {
        if (pressedEscBtn)
        {
            OnSettingsExitDelegate?.Invoke();
            this.enabled = false;
        }
    }

    public void ChangeVolume(int soundType)
    {
        float sliderValue = volumeSliders[soundType].value;
        SoundManager.Instance.SetVolume((SoundType)soundType, ConvertValueToVolume(sliderValue));
        volumeTexts[soundType].text = ((int)(volumeSliders[soundType].value * 100f)).ToString();
    }


    /*
        AudioMixer volume =    -80f ~ 0f
        Slider value      = 0.0001f ~ 1f
    */
    private float ConvertValueToVolume(float value)
    {
        value = Mathf.Clamp(value, SLIDER_MIN, SLIDER_MAX);
        return Mathf.Log10(value) * 20f;
    }
    
    private float ConvertVolumeToValue(float value)
    {
        value = Mathf.Pow(10, value / 20f);
        return Mathf.Clamp(value, SLIDER_MIN, SLIDER_MAX);
    }
}
