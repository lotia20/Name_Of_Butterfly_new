using System.Collections;
using UnityEngine;

public class SideFadeEffect : MonoBehaviour
{
    public CanvasGroup panel1CanvasGroup;
    public CanvasGroup panel2CanvasGroup;
    public float minAlpha = 0.3f;
    public float maxAlpha = 1.0f;
    public float transparencyDuration = 1.0f;
    public float delayBetweenTransparencies = 0.5f;

    private bool isFadingPanel1 = false;
    private bool isFadingPanel2 = false;
    private float startTimePanel1;
    private float startTimePanel2;

    private void Start()
    {
        InvokeRepeating("ToggleTransparency", 0f, transparencyDuration + delayBetweenTransparencies);
    }

    private void ToggleTransparency()
    {
        if (!isFadingPanel1 && !isFadingPanel2)
        {
            StartCoroutine(FadeCanvasGroup(panel1CanvasGroup, minAlpha, true));

            StartCoroutine(FadeCanvasGroup(panel2CanvasGroup, minAlpha, false));

            Invoke("FadeIn", delayBetweenTransparencies);
        }
    }

    private void FadeIn()
    {
        if (!isFadingPanel1 && !isFadingPanel2)
        {
            StartCoroutine(FadeCanvasGroup(panel1CanvasGroup, maxAlpha, true));

            StartCoroutine(FadeCanvasGroup(panel2CanvasGroup, maxAlpha, false));
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha, bool isPanel1)
    {
        if (isPanel1)
        {
            isFadingPanel1 = true;
            startTimePanel1 = Time.time;
        }
        else
        {
            isFadingPanel2 = true;
            startTimePanel2 = Time.time;
        }

        float startAlpha = canvasGroup.alpha;

        while (Time.time < (isPanel1 ? startTimePanel1 : startTimePanel2) + transparencyDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, (Time.time - (isPanel1 ? startTimePanel1 : startTimePanel2)) / transparencyDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;

        if (isPanel1)
        {
            isFadingPanel1 = false;
        }
        else
        {
            isFadingPanel2 = false;
        }
    }
}
