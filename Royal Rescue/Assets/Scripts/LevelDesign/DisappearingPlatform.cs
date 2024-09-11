using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] private MeshRenderer platformRenderer;
    [SerializeField] private BoxCollider platformCollider;
    [SerializeField] private float speed;
    private PlatformState state;
    private Color originalColor1, originalColor2;

    private bool disappearCondition => (originalColor1.a > 0 && originalColor2.a > 0);
    private bool appearCondition =>  (originalColor1.a < 1 && originalColor2.a < 1);

    void Awake()
    {
        originalColor1 = platformRenderer.materials[0].color;
        originalColor2 = platformRenderer.materials[1].color;
        state = PlatformState.DISAPPEAR;
    }

    void Update()
    {
        switch (state)
        {
            case PlatformState.DISAPPEAR:
                Fade(disappearCondition, -speed, state);
                break;
            
            case PlatformState.APPEAR:
                Fade(appearCondition, speed, state);
                break;

            default:
                break;
        }
    }

    public void SetState(PlatformState platformState)
    {
        state = platformState;
    }

    private void Fade(bool isFading, float fadeSpeed, PlatformState platformState)
    {
        if (isFading)
        {
            originalColor1 = new Color(originalColor1.r, originalColor1.g, originalColor1.b, originalColor1.a + fadeSpeed * Time.deltaTime);
            platformRenderer.materials[0].color = originalColor1;

            originalColor2 = new Color(originalColor2.r, originalColor2.g, originalColor2.b, originalColor2.a + fadeSpeed * Time.deltaTime);
            platformRenderer.materials[1].color = originalColor2;
        }
        else
        {
            platformCollider.enabled = (platformState != PlatformState.DISAPPEAR);
            state = PlatformState.DEFAULT;
        }
    }
}
public enum PlatformState { DEFAULT, APPEAR, DISAPPEAR }