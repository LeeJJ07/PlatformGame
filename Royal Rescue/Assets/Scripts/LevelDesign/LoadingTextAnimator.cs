using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingTextAnimator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private float textDelay;
    private string[] loadingTextArr = { "Loading.", "Loading..", "Loading..."};
    int currentIndex = 0;
    void Start()
    {
        InvokeRepeating("AnimateText", 0, 0.5f);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    private void AnimateText()
    {
        loadingText.text = loadingTextArr[currentIndex];
        currentIndex = (currentIndex + 1) % loadingTextArr.Length;
    }
}
