using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartController : MonoBehaviour
{
    FadeEffect fade;

    // Start is called before the first frame update
    void Awake()
    {
        fade = FindObjectOfType<FadeEffect>();
        fade.FadeOut();
    }
}
