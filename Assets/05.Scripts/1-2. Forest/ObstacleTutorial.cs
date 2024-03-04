using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTutorial : MonoBehaviour
{
    public GameObject player;
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;
    public GameObject object4;
    public GameObject object5;

    public GameObject shiftUi;
    public GameObject spaceBarUi;
    public GameObject ctrlUi;
    public GameObject rKeyUi;
    public GameObject leftMouseUi;

    private bool[] eventsCompleted = new bool[5];
    public TutorialExpose tutorialExpose;

    GunChargeController gunChargeController;

    private void Start()
    {
        gunChargeController = FindObjectOfType<GunChargeController>();
    }
    void Update()
    {
        if (!eventsCompleted[0] && IsPlayerNearObject(object1, 50f))
        {
            tutorialExpose.SetImage(shiftUi);
            tutorialExpose.ShowAndHideImage(KeyCode.LeftShift);
            eventsCompleted[0] = true;
        }
        else if (!eventsCompleted[1] && IsPlayerNearObject(object2, 15f) && eventsCompleted[0])
        {
            tutorialExpose.SetImage(spaceBarUi);
            tutorialExpose.ShowAndHideImage(KeyCode.Space);
            eventsCompleted[1] = true;
        }
        else if (!eventsCompleted[2] && IsPlayerNearObject(object3, 15f) && eventsCompleted[1])
        {
            tutorialExpose.SetImage(ctrlUi);
            tutorialExpose.ShowAndHideImage(KeyCode.LeftControl);
            eventsCompleted[2] = true;
        }
        else if (!eventsCompleted[3] && gunChargeController.IsColliding && eventsCompleted[2])
        {
            tutorialExpose.SetImage(rKeyUi);
            tutorialExpose.ShowAndHideImage(KeyCode.R);
            eventsCompleted[3] = true;
        }
        else if (!eventsCompleted[4] && IsPlayerNearObject(object5, 30f) && eventsCompleted[3])
        {
            tutorialExpose.SetImage(leftMouseUi);
            tutorialExpose.ShowAndHideImage(KeyCode.Mouse0);
            eventsCompleted[4] = true;
        }
    }

    bool IsPlayerNearObject(GameObject obj, float distanceThreshold)
    {
        if (obj != null)
        {
            float distance = Vector3.Distance(obj.transform.position, player.transform.position);
            return distance <= distanceThreshold;
        }
        return false;
    }
}

