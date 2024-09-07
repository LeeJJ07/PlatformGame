using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenuControl : MonoBehaviour
{
    private enum MenuState { START, HOWTOPLAY, SETTINGS, EXIT };

    [SerializeField] private List<Image> menuImages;
    [SerializeField] private List<TextMeshProUGUI> menuTexts;
    [SerializeField] private Color highlightMenuColor, highlightTextColor;
    [SerializeField] private int menuIndex = 0;
    private int previousMenuIndex, maxMenuIndex;
    private Color menuColor, textColor;

    void OnEnable()
    {
        Init();
    }
    void Update()
    {
        if (Navigate())
            SelectMenu();
    }

    private void Init()
    {
        menuIndex = previousMenuIndex = 0;
        maxMenuIndex = menuTexts.Count - 1;

        if (menuImages.Count > 0)
            menuColor = menuImages[0].color;

        if (menuTexts.Count > 0)
            textColor = menuTexts[0].color;
    }

    private bool Navigate()
    {
        previousMenuIndex = menuIndex;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            menuIndex += 1;

        else if (Input.GetKeyDown(KeyCode.UpArrow))
            menuIndex -= 1;

        if (previousMenuIndex != menuIndex)
        {
            NormalizeMenu(previousMenuIndex);
            menuIndex = Mathf.Clamp(menuIndex, 0, maxMenuIndex);
            HighlightMenu(menuIndex);
            return true;
        }
        return false;
    }

    private void SelectMenu()
    {
        switch ((MenuState)menuIndex)
        {
            case MenuState.START:
                break;

            case MenuState.HOWTOPLAY:
                break;

            case MenuState.SETTINGS:
                break;

            case MenuState.EXIT:
                QuitGame();
                break;
        }
    }

    private void NormalizeMenu(int index)
    {
        menuImages[index].color = menuColor;
        menuTexts[index].color = textColor;
    }

    private void HighlightMenu(int index)
    {
        menuImages[index].color = highlightMenuColor;
        menuTexts[index].color = highlightTextColor;
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