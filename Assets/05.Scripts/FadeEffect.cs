using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    public CanvasGroup fadeInCanvasGroup;
    public CanvasGroup fadeOutCanvasGroup;
    public float TimeToFade;

    public bool isFadingIn = false;
    public bool isFadingOut = false;

    // Update is called once per frame
    void Update()
    {
        if (isFadingIn)
        {
            if (fadeInCanvasGroup.alpha < 1)
            {
                fadeInCanvasGroup.alpha += TimeToFade * Time.deltaTime;
                if (fadeInCanvasGroup.alpha >= 1)
                {
                    fadeInCanvasGroup.alpha = 1;
                    isFadingIn = false;
                }
            }
        }
        else if (isFadingOut)
        {
            if (fadeOutCanvasGroup.alpha > 0)
            {
                fadeOutCanvasGroup.alpha -= TimeToFade * Time.deltaTime;
                if (fadeOutCanvasGroup.alpha <= 0)
                {
                    fadeOutCanvasGroup.alpha = 0;
                    isFadingOut = false;
                }
            }
        }
    }

    public void FadeIn()
    {
        if (isFadingOut)
        {
            return;
        }

        fadeInCanvasGroup.alpha = 0; 
        fadeInCanvasGroup.gameObject.SetActive(true);
        isFadingIn = true;
    }

    public void FadeOut()
    {
        if (isFadingIn)
        {
            return;
        }

        fadeInCanvasGroup.alpha = 0;
        fadeOutCanvasGroup.alpha = 1; 
        fadeOutCanvasGroup.gameObject.SetActive(true); 
        isFadingOut = true;
    }
}
