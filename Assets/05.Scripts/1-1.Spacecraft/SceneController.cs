using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject door;
    
    void LoadForestScene()
    {
        SceneManager.LoadScene("1-2.Forest");
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == door)
        {
            if(DoorController.doorOpen)
            {
                Debug.Log("load Scene");
                gameObject.GetComponent<PlayerController>().enabled = false;
                LoadForestScene();
            }
        }
    }
}
