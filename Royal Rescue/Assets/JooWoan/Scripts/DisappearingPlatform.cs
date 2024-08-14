using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] private MeshRenderer platformRenderer;
    [SerializeField] private float speed;
    private Color originalColor1, originalColor2;

    [SerializeField] private bool startDisappear;
    [SerializeField] private bool startAppear;
    void Awake()
    {
        originalColor1 = platformRenderer.materials[0].color;
        originalColor2 = platformRenderer.materials[1].color;
    }

    void Update()
    {
        Disappear();
    }

    private void Disappear()
    {
        if (startDisappear && originalColor1.a > 0 && originalColor2.a > 0)
        {
            originalColor1 = new Color(originalColor1.r, originalColor1.g, originalColor1.b, originalColor1.a - speed);
            platformRenderer.materials[0].color = originalColor1;

            originalColor2 = new Color(originalColor2.r, originalColor2.g, originalColor2.b, originalColor2.a - speed);
            platformRenderer.materials[1].color = originalColor2;
        }
        else
            startDisappear = false;
    }
}