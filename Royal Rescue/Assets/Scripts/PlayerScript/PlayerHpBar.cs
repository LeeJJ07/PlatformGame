using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpBar : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Slider _slider;

    public void SetValue(float value)
    {
        _slider.value = value;
    }
}
