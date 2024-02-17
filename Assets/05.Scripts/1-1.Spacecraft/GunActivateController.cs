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
    public AudioClip doorOpenSound;
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
    }

    void GunActivate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PlaySoundAndActivateGun(gunPowerOnSound));
        }
    }

    IEnumerator PlaySoundAndActivateGun(AudioClip sound)
    {
        if (sound != null && gunAudioSource != null)
        {
            gunAudioSource.clip = sound;
            gunAudioSource.Play();
            activateGun.SetActive(true);
            deactivateGun.SetActive(false);
            fullGauge.SetActive(true);
            isGunActivate = true;
            yield return new WaitForSeconds(sound.length); 
            gunAudioSource.Stop();

            StartCoroutine(PlayVibrationSound());
        }
    }

    IEnumerator PlayVibrationSound()
    {
        if (vibrationSound != null && gunAudioSource != null)
        {
            gunAudioSource.clip = vibrationSound;
            gunAudioSource.Play();
            yield return new WaitForSeconds(vibrationSound.length);
            gunAudioSource.Stop();

            StartCoroutine(PlayDoorOpenSound());
        }
    }

        IEnumerator PlayDoorOpenSound()
    {
        if (doorOpenSound != null && gunAudioSource != null)
        {
            gunAudioSource.clip = doorOpenSound;
            gunAudioSource.Play();
            yield return new WaitForSeconds(doorOpenSound.length);
            gunAudioSource.Stop();
        }
    }

}
