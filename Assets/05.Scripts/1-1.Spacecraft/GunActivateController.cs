using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunActiveController : MonoBehaviour
{
    public GameObject deactivateGun;
    public GameObject activateGun;
    public GameObject fullGauge;

    public AudioClip gunPowerOnSound;
    public AudioClip vibrationSound;
    private AudioSource gunAudioSource;

    public static bool isGunActivate{ get; private set; } = false;

    

    void Start()
    {
        activateGun.SetActive(false);
        deactivateGun.SetActive(true); 
        fullGauge.SetActive(false);

        gunAudioSource = gameObject.AddComponent<AudioSource>();
    }
    void Update()
    {
        if (PasswordButtonColorChanger.isBoxOpen)
        {
            Debug.Log("gun Activate");
            GunActivate(); 
        }
        if(CamShake.isShaking)
        {
            PlaySound(vibrationSound);
            Debug.Log("Play vibration Sound");
        }
    }

    void GunActivate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlaySound(gunPowerOnSound);
            activateGun.SetActive(true);
            deactivateGun.SetActive(false);
            fullGauge.SetActive(true);
            isGunActivate = true;
        }
    }

    void PlaySound(AudioClip sound)
    {
        if (sound != null && gunAudioSource != null)
        {
            gunAudioSource.clip = sound;
            gunAudioSource.Play();
        }
    }

}
