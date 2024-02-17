using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public GameObject door;

    void LoadLaboratoryScene()
    {
        SceneManager.LoadScene("1-3.Laboratory");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == door)
        {
            if (DoorOpener.isDoorOpen)
            {
                Debug.Log("load Scene");
                gameObject.GetComponent<PlayerController>().enabled = false;
                LoadLaboratoryScene();
            }
        }
    }
}
