using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMenu : MonoBehaviour
{
    [SerializeField] protected List<Image> menuImages;
    [SerializeField] protected List<TextMeshProUGUI> menuTexts;
    [SerializeField]protected Color menuColor, highlightMenuColor;
    [SerializeField] protected Color textColor, highlightTextColor;
    protected int menuIndex = 0;
    protected int previousMenuIndex, maxMenuIndex;
    public static bool pressedConfirmBtn => Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Attack");

    protected virtual void Start()
    {
        Init();
        SetMenuHighlight(0, true);
    }

    protected void Init()
    {
        menuIndex = previousMenuIndex = 0;
        maxMenuIndex = menuTexts.Count - 1;
    }

    protected void Navigate(KeyCode increaseKey = KeyCode.DownArrow, KeyCode decreaseKey = KeyCode.UpArrow)
    {
        previousMenuIndex = menuIndex;

        if (Input.GetKeyDown(increaseKey))
            menuIndex += 1;

        else if (Input.GetKeyDown(decreaseKey))
            menuIndex -= 1;

        if (previousMenuIndex != menuIndex)
        {
            SetMenuHighlight(previousMenuIndex, false);
            menuIndex = Mathf.Clamp(menuIndex, 0, maxMenuIndex);
            SetMenuHighlight(menuIndex, true);
        }
    }

    protected void SetMenuHighlight(int index, bool isHighlighted)
    {
        Color targetMenuColor = isHighlighted ? highlightMenuColor : menuColor;
        Color targetTextColor = isHighlighted ? highlightTextColor : textColor;

        menuImages[index].color = targetMenuColor;
        menuTexts[index].color = targetTextColor;
    }
}
