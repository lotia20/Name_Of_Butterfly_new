using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlMenu : MonoBehaviour
{
    //fadeIn 추가 부분
    FadeEffect fade;

    void Start()
    {
        fade = FindObjectOfType<FadeEffect>();
    }

    public IEnumerator FadeScene(int sceneIndex)
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneIndex);
    }


    public void OnClickStart()
    {
        Debug.Log("Start");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        StartCoroutine(FadeScene(nextSceneIndex));
        // SceneManager.LoadScene(nextSceneIndex);
    }

    public void OnClickSetting()
    {
        Debug.Log("Setting");
    }

    public void OnClickAcheivement()
    {
        Debug.Log("Achievement");
    }

    public void OnClickExit()
    {
        Debug.Log("Exit");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}