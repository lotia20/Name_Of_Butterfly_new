using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialExpose : MonoBehaviour
{
    public GameObject imageToShow;
    public float displayTime = 5f;

    private KeyCode hideKeyCode;

    public void SetImage(GameObject image)
    {
        imageToShow = image;
    }

    public void ShowAndHideImage(KeyCode keyCode)   //Ư�� Ű ������ UI ��Ȱ��ȭ
    {
        ShowImage();
        hideKeyCode = keyCode;
        StartCoroutine(WaitAndHide());
    }
    public void ShowAndHideImage()  //displayTime ��ŭ�� �帣�� �ڵ����� ��Ȱ��ȭ 
    {
        ShowImage();
        Invoke("HideImage", displayTime);
    }

    IEnumerator WaitAndHide()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(hideKeyCode));
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


