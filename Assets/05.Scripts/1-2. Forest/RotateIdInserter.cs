using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RotateIdInserter : MonoBehaviour
{
    public static bool IsRockRotated { get; private set; } = false;

    public UnityEvent RockRotated = new UnityEvent();

    //추후 enable 넣기 
    //public CleaningScript cleaningScript;
    public bool cleanComplete = true;
    [SerializeField]private float interactDistance;
    public GameObject idInserter;
    public GameObject player;

    public static Vector3 originalCameraPosition { get; private set; }
    public static Quaternion originalCameraRotation { get; private set; }


    private void Update()
    {
        if (CanInteract() && Input.GetKeyDown(KeyCode.E) && !IsRockRotated)
        {
            player.GetComponent<PlayerController>().enabled = false;
            StartCoroutine(SequentialRockRotation(idInserter));
        }
    }

    private bool CanInteract()
    {
        if (!cleanComplete || idInserter == null || player == null)
        {
            return false;
        }

        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(idInserter.transform.position);
        bool inCameraView = viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0;
        float distanceToPlayer = Vector3.Distance(idInserter.transform.position, player.transform.position);
        return inCameraView && distanceToPlayer <= interactDistance;
    }

    IEnumerator SequentialRockRotation(GameObject idInserter)
    {
        yield return StartCoroutine(MoveCameraToLookAtObject(idInserter.transform));
        yield return StartCoroutine(RotateRock(idInserter));
        yield return StartCoroutine(ResetCameraPositionAndRotation());
        player.GetComponent<PlayerController>().enabled = true;
    }

    IEnumerator RotateRock(GameObject obj)
    {
        float rotationDuration = 4.0f;
        float elapsedTime = 0f;
        Quaternion startRotation = obj.transform.rotation;

        Quaternion targetRotation = startRotation * Quaternion.Euler(0f, 180f, 20f);

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / rotationDuration;
            obj.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }
        obj.transform.rotation = targetRotation;
    }

    IEnumerator MoveCameraToLookAtObject(Transform obj)
    {
        SaveOriginalCameraTransform();

        Vector3 directionToObj = obj.position - Camera.main.transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(directionToObj);
        float rotationSpeed = 5.0f;

        while (Quaternion.Angle(Camera.main.transform.rotation, desiredRotation) > 0.01f)
        {
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);

            yield return null;
        }
    }

    void SaveOriginalCameraTransform()
    {
        originalCameraPosition = Camera.main.transform.position;
        originalCameraRotation = Camera.main.transform.rotation;
    }

    IEnumerator ResetCameraPositionAndRotation()
    {
        float duration = 1.0f;
        float elapsedTime = 0f;
        Vector3 startCameraPosition = Camera.main.transform.position;
        Quaternion startCameraRotation = Camera.main.transform.rotation;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            Camera.main.transform.position = Vector3.Lerp(startCameraPosition, originalCameraPosition, t);
            Camera.main.transform.rotation = Quaternion.Lerp(startCameraRotation, originalCameraRotation, t);
            yield return null;
        }

        Camera.main.transform.position = originalCameraPosition;
        Camera.main.transform.rotation = originalCameraRotation;
    }

    public void OnRockRotated()
    {
        IsRockRotated = true;
        RockRotated.Invoke();
    }
}

