using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialManager : MonoBehaviour
{
    public GameObject tutorial;

    FadeEffect fade;

    // Start is called before the first frame update
    void Start()
    {
        tutorial.SetActive(false);
        //fadeOut 추가 부분
        fade = FindObjectOfType<FadeEffect>();
        fade.FadeOut();
        
    }

    // Update is called once per frame
    void Update()
    {  
        if(Input.GetKey(KeyCode.Escape))
        {
            tutorial.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            tutorial.SetActive(false);
        }
        
    }
}
