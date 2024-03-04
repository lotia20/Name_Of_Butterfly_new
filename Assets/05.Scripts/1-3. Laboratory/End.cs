using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    FadeEffect fade;
    public GameObject door;

    void Start()
    {
        fade = FindObjectOfType<FadeEffect>();
    }

    public IEnumerator FadeScene(string sceneName)
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == door)
        {
          
                Debug.Log("load Scene");
                gameObject.GetComponent<PlayerController>().enabled = false;

                StartCoroutine(FadeScene("0.Start"));
            
        }
    }
}