using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeInOut
{
    public static IEnumerator Fade(Image targetImage, bool isFadeIn, float fadeInSpeed, float fadeOutSpeed)
    {
        float initialAlpha = 1f;
        float multiplier = -1f;
        float fadeSpeed = isFadeIn ? fadeInSpeed : fadeOutSpeed;

        if (!isFadeIn)
        {
            initialAlpha = 0f;
            multiplier = 1f;
        }
        Color tempColor = targetImage.color;
        tempColor.a = initialAlpha;
        targetImage.color = tempColor;

        while (true)
        {
            tempColor = targetImage.color;
            tempColor.a += multiplier * fadeSpeed * Time.deltaTime;
            targetImage.color = tempColor;

            if (isFadeIn && targetImage.color.a <= 0f) break;
            else if (!isFadeIn && targetImage.color.a >= 1f) break;

            yield return null;
        }
    }

    public static IEnumerator Fade(TextMeshProUGUI targetText, bool isFadeIn, float fadeSpeed)
    {
        targetText.alpha = 0f;
        float multiplier = 1f;
        
        if (!isFadeIn)
        {
            targetText.alpha = 1f;
            multiplier = -1f;
        }
        while (true)
        {
            targetText.alpha += multiplier * fadeSpeed * Time.deltaTime;

            if (isFadeIn && targetText.alpha >= 1f) break;
            else if (!isFadeIn && targetText.alpha <= 0f) break;

            yield return null;
        }
    }

    public static IEnumerator Fade(CanvasGroup group, bool isFadeIn, float fadeSpeed)
    {
        group.alpha = 0f;
        float multiplier = 1f;
        
        if (!isFadeIn)
        {
            group.alpha = 1f;
            multiplier = -1f;
        }
        while (true)
        {
            group.alpha += multiplier * fadeSpeed * Time.deltaTime;

            if (isFadeIn && group.alpha >= 1f) break;
            else if (!isFadeIn && group.alpha <= 0f) break;

            yield return null;
        }
    }
}
