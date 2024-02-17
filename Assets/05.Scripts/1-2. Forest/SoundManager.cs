using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManger : MonoBehaviour
{
    public GameObject box;
    public GameObject leave;
    public AudioClip boxSound;
    public AudioClip leaveSound;
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == box)
        {
            PlaySound(boxSound);
        }
        if(other.gameObject == leave)
        {
            PlaySound(leaveSound);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject == box)
        {
            audioSource.Stop(); 
        }
        if(other.gameObject == leave)
        {
            audioSource.Stop(); 
        }
    }

    void PlaySound(AudioClip sound)
    {
        if (sound != null && audioSource != null)
        {
            audioSource.clip = sound;
            audioSource.Play();
        }
    }
}
