using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    public CanvasGroup fadeInCanvasGroup;
    public CanvasGroup fadeOutCanvasGroup;
    public bool fadeIn = false;
    public bool fadeOut = false;
    public float TimeToFade;

    // Update is called once per frame
    void Update()
    {
        if(fadeIn == true)
        {
            if(fadeInCanvasGroup.alpha < 1)
            {
                fadeInCanvasGroup.alpha += TimeToFade * Time.deltaTime;
                if(fadeInCanvasGroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }
        if(fadeOut == true)
        {
            if(fadeOutCanvasGroup.alpha >= 0)
            {
                fadeOutCanvasGroup.alpha -= TimeToFade * Time.deltaTime;
                if(fadeOutCanvasGroup.alpha == 0)
                {
                    fadeOut = false;
                }
            }
        }
    }
    public void FadeIn()
    {
        fadeIn = true;
    }

    public void FadeOut()
    {
        fadeOut = true;
    }
}
