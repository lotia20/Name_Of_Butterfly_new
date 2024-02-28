using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUp : MonoBehaviour
{
    //GameObject
    public GameObject cover;
    public GameObject player;
    public Camera camera;
    public GameObject ArmL;
    public GameObject ArmR;

    //Angle & Speed
    [SerializeField] private float openAngle;
    [SerializeField] private float bowAngle;
    [SerializeField] private float openSpeed;
    [SerializeField] private float wakeUpSpeed;
    [SerializeField] private float handCheckSpeed;
   
   //Cover Rotation
    private Quaternion closedRotation;
    private Quaternion openRotation; 

    //Player Rotation
    private Quaternion lieRotation;
    private Quaternion standRotation;
    private Quaternion sitRotation;

    //Camera Rotation
    private Quaternion cameraRotation;
    private Quaternion bowCameraRotation;

    //ArmRotation
    private Quaternion leftArmRotation;
    private Quaternion rightArmRotation;
    private Quaternion raiseUpLeftArmRotation;
    private Quaternion raiseUpRightArmRotation;

    //Audio
    public AudioClip wakeUpSound;
    public AudioClip clothSound;
    public AudioClip clothMovingSound;
    private AudioSource audioSource;

    public BasicTutorial basicTutorial;

    private bool isWakeUpComplete = false;

    void Start()
    {
        player.GetComponent<PlayerController>().enabled = false;
        closedRotation = cover.transform.rotation;
        openRotation = Quaternion.Euler(0f, 0f, openAngle);

        lieRotation = player.transform.rotation;
        standRotation = Quaternion.Euler(0f, 0f, 0f);
        sitRotation = Quaternion.Euler(0f, 90f, 0f);

        bowCameraRotation = Quaternion.Euler(bowAngle, 90f, 0f);

        raiseUpLeftArmRotation = Quaternion.Euler(36f, 0f, 173f);
        raiseUpRightArmRotation = Quaternion.Euler(-36f, 0f, 173f);
        audioSource = gameObject.AddComponent<AudioSource>();
        
        StartCoroutine(WakeUpSequence());
    }

    void Update()
    {
        if (isWakeUpComplete)
        {
            basicTutorial.CheckInputs();
        }
    }

    // Update is called once per frame
    IEnumerator WakeUpSequence()
    {
        yield return StartCoroutine(OpenCover());
        yield return StartCoroutine(StandUp());
        yield return StartCoroutine(SitDown());
        yield return StartCoroutine(CheckHand());
        yield return StartCoroutine(WakeUpComplete());
    }

    IEnumerator OpenCover()
    {
        Debug.Log("open");
        float elapsedTime = 0f;

        PlaySound(clothSound);

        yield return new WaitForSeconds(1f);
        

        PlaySound(wakeUpSound);

        while(elapsedTime < openSpeed)
        {
            cover.transform.rotation = Quaternion.Lerp(closedRotation, openRotation, elapsedTime / openSpeed);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        cover.transform.rotation = openRotation;
    }
    IEnumerator StandUp()
    {
        Debug.Log("Stand up");
        float elapsedTime = 0f;

        PlaySound(clothMovingSound);

        while(elapsedTime < wakeUpSpeed)
        {
            player.transform.rotation = Quaternion.Lerp(lieRotation, standRotation, elapsedTime / wakeUpSpeed);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        player.transform.rotation = standRotation;
    }

    IEnumerator SitDown()
    {
        Debug.Log("Sit Down");
        float elapsedTime = 0f;

        while(elapsedTime < wakeUpSpeed)
        {
            player.transform.rotation = Quaternion.Lerp(standRotation, sitRotation, elapsedTime / wakeUpSpeed);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        player.transform.rotation = sitRotation;
        leftArmRotation = ArmL.transform.rotation;
        rightArmRotation = ArmR.transform.rotation;
        audioSource.Stop();
    }

    IEnumerator CheckHand()
    {
        Debug.Log("Raise Up Arm");
        ArmL.transform.parent = null;
        ArmL.transform.rotation = raiseUpLeftArmRotation;
        
        ArmR.transform.parent = null;
        ArmR.transform.rotation = raiseUpRightArmRotation;

        Debug.Log("Bow Head");
        PlaySound(clothMovingSound);
        
        cameraRotation = camera.transform.rotation;
        camera.transform.parent = null;

        float elapsedTime = 0f;

        while(elapsedTime < handCheckSpeed)
        {
            camera.transform.rotation = Quaternion.Lerp(cameraRotation, bowCameraRotation, elapsedTime / handCheckSpeed);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        camera.transform.rotation = bowCameraRotation;

        audioSource.Stop();

        yield return new WaitForSeconds(1);

    }
    IEnumerator WakeUpComplete()
    {
        ArmL.transform.parent = player.transform;
        ArmL.transform.rotation = leftArmRotation;
        ArmR.transform.parent = player.transform;
        ArmR.transform.rotation = rightArmRotation;
        camera.transform.parent = player.transform;
        camera.transform.rotation = cameraRotation;

        player.transform.position = new Vector3(-1.438f, 0.733f, -0.031f);

        player.GetComponent<PlayerController>().enabled = true;
        isWakeUpComplete = true;

        yield return null;
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
