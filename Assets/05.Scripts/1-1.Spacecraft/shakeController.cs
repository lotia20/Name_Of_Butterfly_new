using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shakeController : MonoBehaviour
{
    private CamShake camShake;
    public AudioClip vibrationSound;
    private AudioSource shakingSource;

    public GameObject SpaceCraft;

  
    void Start()
    {
        camShake = SpaceCraft.GetComponent<CamShake>();
        shakingSource = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GunActiveController.isGunActivate)
        {
            Debug.Log("shakeController.cs start");
            StartCoroutine(camShake.ShakeAfterDelay(3f));
        }
        if(CamShake.isShaking)
        {
            Debug.Log("isShaking = true");
            PlaySound(vibrationSound);
            if (shakingSource.isPlaying)
            {
                Debug.Log("사운드가 재생 중입니다.");
            }
        }
        if(!CamShake.isShaking)
        {
            Debug.Log("shakingSource stop");
            shakingSource.Stop();
        }
        
    }

    void PlaySound(AudioClip sound)
    {
        if (sound != null && shakingSource != null)
        {
            shakingSource.clip = sound;
            shakingSource.Play();
        }
    }
}
