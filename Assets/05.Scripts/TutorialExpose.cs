using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialExpose : MonoBehaviour
{
    public GameObject imageToShow;
    public float displayTime = 5f;
    public Color greenColor = Color.green;

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
        imageToShow.gameObject.SetActive(true);
    }

    public void HideImage()
    {
        imageToShow.gameObject.SetActive(false);
    }
}


