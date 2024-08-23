using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChargeController : MonoBehaviour
{
    public GameObject player;
    public GameObject camera;
    public GameObject waterWall;
    public GameObject targetPosition;

    public GameObject[] gauges;
    public int gaugeIndex;

    private Quaternion originRotation;
    private Vector3 originPosition;
    private float rotationSpeed = 30f;
    private float moveSpeed = 1f;

    private bool isColliding = false;
    public static bool isCharging{ get; set; } = false;
    [SerializeField] private float gaugeChargeDelay = 0.5f;

    public AudioClip chargingSound;
    public AudioClip chargedSound;
    private AudioSource audioSource;


    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        InitilalizeGauges();
    }

    // Update is called once per frame
    void Update()
    {
        if(isColliding && Input.GetKeyDown(KeyCode.R))
        {
            player.GetComponent<PlayerController>().enabled = false;
            player.transform.rotation = Quaternion.Euler(0f, 2500f, 0f);
            FixCameraPosition();
        }
        // if(CleanController.isDecreasing && !CleanController.isCleaning)
        // {
        //     DecreaseGauge();
        //     CleanController.isDecreasing = false;
        // }
    }

    void InitilalizeGauges()
    {
        for(int i = 0; i < gauges.Length; i++)
        {
            gauges[i].SetActive(false);
        }
        gaugeIndex = 0;
        gauges[gaugeIndex].SetActive(true);
    }

    void FixCameraPosition()
    {
        Debug.Log("Fix");
        StartCoroutine(RotateAndMoveCamera());
    }

    IEnumerator RotateAndMoveCamera()
    {
        camera.transform.parent = null;

        originPosition = camera.transform.position;
        originRotation = camera.transform.rotation;
        Debug.Log(originPosition);
        
        Quaternion targetRotation = Quaternion.Euler(50f, 0f, 0f);

        while(camera.transform.rotation != targetRotation)
        {
            camera.transform.rotation = Quaternion.RotateTowards(camera.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        while(camera.transform.position.y > targetPosition.transform.position.y)
        {
            camera.transform.position -= Vector3.up * moveSpeed * Time.deltaTime;
            yield return null;
        }

        ChargeGun();
    }

    void ChargeGun()
    {
        Debug.Log("Gun charged");

        StartCoroutine(ActivateGauges());
    }

    IEnumerator ActivateGauges()
    {
        PlaySound(chargingSound);
        for(int  i = gaugeIndex; i <= 10; i++)
        {
            gauges[i].SetActive(true);
            DisableOtherGauges(i);

            yield return new WaitForSeconds(gaugeChargeDelay);
        }
        gaugeIndex = 10;
        audioSource.Stop();
        isCharging = true;
        ResetCamera();
    }

    // public void DecreaseGauge()
    // {
    //     if(gaugeIndex >= 1)
    //     {
    //         gaugeIndex -= 1;
    //         gauges[gaugeIndex].SetActive(true);
    //         DisableOtherGauges(gaugeIndex);
    //     }
    //     else
    //     {
    //         isCharging = false;
    //     }
    
    // }

    void DisableOtherGauges(int currentIndex)
    {
        for(int j = 1; j < gauges.Length; j++)
        {
            if(j != currentIndex)
            {
                gauges[j].SetActive(false);
            }
        }
    }

    void ResetCamera()
    {
        PlaySound(chargedSound);
        camera.transform.parent = player.transform;
        camera.transform.rotation = originRotation;
        camera.transform.position = originPosition;

        player.GetComponent<PlayerController>().enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == waterWall)
        {
            isColliding = true;
            Debug.Log("Trigger Enter");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject == waterWall)
        {
            isColliding = false;
            Debug.Log("Trigger Exit");
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
