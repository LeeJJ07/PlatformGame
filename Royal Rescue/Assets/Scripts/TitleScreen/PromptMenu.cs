using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptMenu : UIMenu
{
    private enum PromptType { CONFIRM, CANCEL };
    public delegate void OnPromptCancel();
    public delegate void OnPromptConfirm();
    public OnPromptCancel OnCancelDelegate { get; set; }
    public OnPromptConfirm OnConfirmDelegate { get; set; }

    protected void OnEnable()
    {
        SetMenuHighlight(menuIndex, false);
        base.Start();
    }

    protected void Update()
    {
        Navigate(KeyCode.RightArrow, KeyCode.LeftArrow);
        SelectMenu();
    }

    protected void SelectMenu()
    {
        if (pressedConfirmBtn)
        {
            switch ((PromptType)menuIndex)
            {
                case PromptType.CONFIRM:
                    OnConfirmDelegate?.Invoke();
                    break;

                case PromptType.CANCEL:
                    OnCancelDelegate?.Invoke();
                    break;

                default:
                    break;
            }
            this.enabled = false;
        }
    }
}
