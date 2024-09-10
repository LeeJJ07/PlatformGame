using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenu : UIMenu
{
    public delegate void OnHelpCancel();
    public OnHelpCancel OnHelpCancelDelegate { get; set; }

    void Update()
    {
        if (pressedEscBtn)
        {
            OnHelpCancelDelegate?.Invoke();
            this.enabled = false;
        }
    }
}
