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

    void OnEnable()
    {
        SetMenuHighlight(menuIndex, false);
        base.Start();
    }

    void Update()
    {
        Navigate(KeyCode.RightArrow, KeyCode.LeftArrow);
        SelectMenu();
    }

    private void SelectMenu()
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
        else if (pressedEscBtn)
        {
            OnCancelDelegate?.Invoke();
            this.enabled = false;
        }
    }
}
