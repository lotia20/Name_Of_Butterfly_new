using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTutorial : MonoBehaviour
{
    public TutorialExpose tutorialExpose;

    public GameObject movementUi;
    public GameObject uiA;
    public GameObject uiW;
    public GameObject uiS;
    public GameObject uiD;
    public GameObject scrollUi;

    private bool eventA = false;
    private bool eventW = false;
    private bool eventS = false;
    private bool eventD = false;
    private bool eventScroll = false;

    void Start()
    {
        movementUi.SetActive(false);
        uiA.SetActive(false);
        uiW.SetActive(false);
        uiS.SetActive(false);
        uiD.SetActive(false);
    }


    public void CheckInputs()
    {
        if (!AllEventsCompleted())
        {
            tutorialExpose.SetImage(movementUi);
            tutorialExpose.ShowImage();
        }

        if (Input.GetKeyDown(KeyCode.A) && !eventA)
        {
            uiA.SetActive(true);
            eventA = true;
        }

        if (Input.GetKeyDown(KeyCode.W) && !eventW)
        {
            uiW.SetActive(true);
            eventW = true;
        }

        if (Input.GetKeyDown(KeyCode.S) && !eventS)
        {
            uiS.SetActive(true);
            eventS = true;
        }

        if (Input.GetKeyDown(KeyCode.D) && !eventD)
        {
            uiD.SetActive(true);
            eventD = true;
        }

        if (eventA && eventW && eventS && eventD && !eventScroll)
        {
            uiA.SetActive(false);
            uiW.SetActive(false);
            uiS.SetActive(false);
            uiD.SetActive(false);
            tutorialExpose.HideImage();
            tutorialExpose.SetImage(scrollUi);
            StartCoroutine(ScrollDetection());
        }
    }
    IEnumerator ScrollDetection()
    {
        bool scrollingDetected = false;
        while (!scrollingDetected)
        {
            tutorialExpose.ShowImage();
            if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0)
            {
                scrollingDetected = true;
                tutorialExpose.HideImage();
                eventScroll = true;
            }
            yield return null;
        }
    }
    public bool AllEventsCompleted()
    {
        return eventA && eventW && eventS && eventD && eventScroll;
    }
}

