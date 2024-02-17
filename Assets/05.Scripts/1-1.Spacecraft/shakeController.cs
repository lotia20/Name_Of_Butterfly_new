// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class shakeController : MonoBehaviour
// {
//     private CamShake camShake;

//     public GameObject SpaceCraft;

//     public AudioClip vibrationSound;
//     private AudioSource vibrationSource;

//     public static bool isShaking{ get; private set; } = false;


  
//     void Start()
//     {
//         camShake = SpaceCraft.GetComponent<CamShake>();
//         vibrationSource = gameObject.GetComponent<AudioSource>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (GunActiveController.isGunActivate)
//         {
//             StartCoroutine(PlayAfterDelay(1f));
//         }
        
//     }

//     public IEnumerator PlayAfterDelay(float delay)
//     {
//         Debug.Log("Play");
//         yield return new WaitForSeconds(delay);
//         PlaySound(vibrationSound);
//     }

//     // void PlaySound(AudioClip sound)
//     // {
//     //     if (sound != null && vibrationSource != null)
//     //     {
//     //         vibrationSource.clip = sound;
//     //         vibrationSource.Play();
//     //     }
//     // }

//     IEnumerator PlaySound(AudioClip sound)
//     {
//         if (sound != null && vibrationSource != null)
//         {
//             vibrationSource.clip = sound;
//             vibrationSource.Play();
//             yield return new WaitForSeconds(sound.length); 
//             isShaking = true;
//             vibrationSource.Stop();
//         }
//     }
// }
