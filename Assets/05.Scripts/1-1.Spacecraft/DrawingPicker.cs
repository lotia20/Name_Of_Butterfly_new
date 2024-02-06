using System;
using UnityEngine;

public class DrawingPicker : MonoBehaviour
{

    public AudioClip frontSound;  // 정면일 때 재생할 사운드
    public AudioClip backSound;   // 뒤로 갈 때 재생할 사운드
    public AudioClip resetSound;  // 위치 초기화할 때 재생할 사운드

    public float xDistanceThreshold = 2f;
    public float activationDistance = 2f;
    float distanceToCamera = 0.4f;

    private bool isObjectFacingFront = false;
    private bool isObjectFacingBack = false;
    private bool hasPerformedBoxOpenEvent = false;

    private OutlineSelection outlineSelection;
    private AudioSource audioSource;

    Vector3[] originalPositions;
    Quaternion[] originalRotations;


    void Start()
    {

        outlineSelection = GetComponent<OutlineSelection>();

        if (outlineSelection == null)
        {
            outlineSelection = gameObject.AddComponent<OutlineSelection>();
        }
        SaveOriginalTransforms();

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void SaveOriginalTransforms()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("SelectableDrawing");
        originalPositions = new Vector3[objects.Length];
        originalRotations = new Quaternion[objects.Length];

        for (int i = 0; i < objects.Length; i++)
        {
            originalPositions[i] = objects[i].transform.position;
            originalRotations[i] = objects[i].transform.rotation;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (OutlineSelection.IsOutlineEnabled && !PasswordEventCameraController.IsPasswordActive)
            {
                GameObject closestObject = OutlineSelection.ClosestObject;

                if (closestObject != null && closestObject.CompareTag("SelectableDrawing"))
                {
                    Camera mainCamera = Camera.main;
                    Vector3 viewportPoint = mainCamera.WorldToViewportPoint(closestObject.transform.position);

                    if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0)
                    {
                        if (isObjectFacingFront)
                        {
                            RotateObject(closestObject);
                            isObjectFacingFront = false;
                            isObjectFacingBack = true;
                            PlaySound(backSound);

                        }
                        else if (isObjectFacingBack)
                        {
                            ResetObjectPosition(closestObject);
                            isObjectFacingBack = false;
                            PlaySound(resetSound);
                        }
                        else
                        {
                            LookAtObjectFront(closestObject);
                            isObjectFacingFront = true;
                            PlaySound(frontSound);
                        }
                    }

                    // 로그 출력 - 나중에
                    Debug.Log("ClosestObject found: " + closestObject.name + ". isObjectFacingFront: " + isObjectFacingFront + ", isObjectFacingBack: " + isObjectFacingBack);
                }
            }
            else
            {
                Debug.Log("Outline is not enabled.");
            }
        }

        if (PasswordButtonColorChanger.isBoxOpen && !hasPerformedBoxOpenEvent)
        {
            GameObject closestObject = OutlineSelection.ClosestObject;
            if (closestObject != null && closestObject.CompareTag("SelectableDrawing"))
            {
                MoveObjectToSpecificPosition(closestObject, new Vector3(-4.341f, 0.758f, -3.059f), closestObject.transform.rotation);

                hasPerformedBoxOpenEvent = true;
            }
        }
    }

    // 오브젝트 - 정면이동
    void LookAtObjectFront(GameObject obj)
    {
        Vector3 targetPosition = Camera.main.transform.position + Camera.main.transform.forward * distanceToCamera;
        obj.transform.position = targetPosition;

        // 정면 로테이션
        Quaternion localRotation = Quaternion.Euler(0, 0, -90);
        Quaternion targetRotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up) * localRotation;

        obj.transform.rotation = targetRotation;
    }

    void ResetObjectPosition(GameObject obj)
    {
        int index = System.Array.IndexOf(GameObject.FindGameObjectsWithTag("SelectableDrawing"), obj);
        obj.transform.position = originalPositions[index];
        obj.transform.rotation = originalRotations[index];
    }

    void RotateObject(GameObject obj)
    {
        Vector3 currentRotation = obj.transform.rotation.eulerAngles;
        currentRotation.y += 180f;
        currentRotation.z = -90f;
        obj.transform.rotation = Quaternion.Euler(currentRotation);
        obj.transform.LookAt(Camera.main.transform.position);

        currentRotation = obj.transform.rotation.eulerAngles;
        currentRotation.z = -90f;
        obj.transform.rotation = Quaternion.Euler(currentRotation);

        Debug.Log(obj.transform.rotation.eulerAngles);
    }
    void PlaySound(AudioClip sound)
    {
        if (sound != null && audioSource != null)
        {
            audioSource.clip = sound;
            audioSource.Play();
        }
    }
    void MoveObjectToSpecificPosition(GameObject obj, Vector3 newPosition, Quaternion originalRotation)
    {
        obj.transform.position = newPosition;
        obj.transform.rotation = originalRotation;
        SaveOriginalTransforms();
    }
}