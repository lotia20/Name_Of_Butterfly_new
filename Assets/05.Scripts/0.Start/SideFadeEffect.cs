using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideFadeEffect : MonoBehaviour
{
    public CanvasGroup panelCanvasGroup;
    public float minAlpha = 0.3f;
    public float maxAlpha = 1.0f;
    public float transparencyDuration = 1.0f;
    public float delayBetweenTransparencies = 1.0f;

    private bool isFading = false;
    private float startTime;

    private void Start()
    {
        InvokeRepeating("ToggleTransparency", 0f, transparencyDuration + delayBetweenTransparencies);
    }

    private void ToggleTransparency()
    {
        if (!isFading)
        {
            // Fade Out
            StartCoroutine(FadeCanvasGroup(minAlpha));

            // Fade In
            Invoke("FadeIn", delayBetweenTransparencies);
        }
    }

    private void FadeIn()
    {
        if (!isFading)
        {
            StartCoroutine(FadeCanvasGroup(maxAlpha));
        }
    }

    private IEnumerator FadeCanvasGroup(float targetAlpha)
    {
        isFading = true;
        float startAlpha = panelCanvasGroup.alpha;
        startTime = Time.time;

        while (Time.time < startTime + transparencyDuration)
        {
            panelCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, (Time.time - startTime) / transparencyDuration);
            yield return null;
        }

        panelCanvasGroup.alpha = targetAlpha;
        isFading = false;
    }
}