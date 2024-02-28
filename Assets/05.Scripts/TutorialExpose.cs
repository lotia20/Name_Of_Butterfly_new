using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialExpose : MonoBehaviour
{
    public GameObject imageToShow;
    public float displayTime = 5f;
    public Color greenColor = Color.green;
    public float minAlpha = 0.3f; 
    public float maxAlpha = 1f; 
    public float fadeDuration = 1f; 

    private KeyCode hideKeyCode;
    private Image imageComponent;

    public void SetImage(GameObject image)
    {
        imageToShow = image;
        imageComponent = image.GetComponent<Image>();
    }

    public void ShowAndHideImage(KeyCode keyCode)   //특정 키 누르면 UI 비활성화
    {
        ShowImage();
        hideKeyCode = keyCode;
        StartCoroutine(WaitAndHide());
    }

    public void ShowAndHideImage()  //displayTime 만큼이 흐르면 자동으로 비활성화 
    {
        ShowImage();
        Invoke("HideImage", displayTime);
    }

    IEnumerator WaitAndHide()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(hideKeyCode));
        imageComponent.color = greenColor;
        yield return new WaitForSeconds(1f);
        HideImage();
    }

    public void ShowImage()
    {
        imageToShow.SetActive(true);
        StartCoroutine(FadeInOut());
    }

    public void HideImage()
    {
        imageToShow.SetActive(false);
    }

    IEnumerator FadeInOut()
    {
        while (true)
        {
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                float alpha = Mathf.Lerp(minAlpha, maxAlpha, elapsedTime / fadeDuration);
                imageComponent.color = new Color(imageComponent.color.r, imageComponent.color.g, imageComponent.color.b, alpha);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            imageComponent.color = new Color(imageComponent.color.r, imageComponent.color.g, imageComponent.color.b, maxAlpha);

            elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                float alpha = Mathf.Lerp(maxAlpha, minAlpha, elapsedTime / fadeDuration);
                imageComponent.color = new Color(imageComponent.color.r, imageComponent.color.g, imageComponent.color.b, alpha);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            imageComponent.color = new Color(imageComponent.color.r, imageComponent.color.g, imageComponent.color.b, minAlpha);
        }
    }
}



