using UnityEngine;
using System.Collections;

public class PasswordEventCameraController : MonoBehaviour
{
    public static bool IsPasswordActive { get; private set; } = false;
    public float distanceToCamera = 2f;
    private bool hasIDCardInserted = false;

    public static Vector3 originalCameraPosition { get; private set; }
    public static Quaternion originalCameraRotation { get; private set; }

    private GameObject idCardObject;
    public GameObject player;
    private AudioSource audioSource;

    private void Start()
    {
        idCardObject = GameObject.FindGameObjectWithTag("IdCard");
        if (idCardObject != null)
        {
            idCardObject.SetActive(false); 
        }
    }

    void Update()
    {
        GameObject closestObject = OutlineSelection.ClosestObject;
        if (Input.GetKeyDown(KeyCode.E) && closestObject != null && closestObject.CompareTag("SelectablePasswordScreen"))
        {
            if (!IsPasswordActive && IDCardPickupEvent.IdCardPickedUp && !hasIDCardInserted)
            {
                StartCoroutine(ActivateIDCardSequence(idCardObject));               
            }
            else
            {
                HandlePasswordActivation();
            }
        }
        if (PasswordButtonColorChanger.isBoxOpen)
        {
            IsPasswordActive = false;
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
    IEnumerator ActivateIDCardSequence(GameObject idCardObject)
    {
        ActivateIDCard();
        MoveCameraToDesiredPosition();
        yield return StartCoroutine(InsertIdCard(idCardObject));
        PlaySound(idCardObject);
        yield return new WaitForSeconds(3f);
        ResetCameraPositionAndRotation();
        HandlePasswordActivation();
        DeactivateIDCard();
        hasIDCardInserted = true;
    }

    void ActivateIDCard()
    {
        if (idCardObject != null)
        {
            idCardObject.SetActive(true);
            player.GetComponent<PlayerController>().enabled = false;
        }
    }

    void DeactivateIDCard()
    {
        if (idCardObject != null)
        {
            idCardObject.SetActive(false);
        }
    }
    void MoveCameraToDesiredPosition()
    {
        SaveOriginalCameraTransform();
        Vector3 desiredPosition = new Vector3(-4.682329f, 1.3411042f, -2.640926f);
        Quaternion desiredRotation = Quaternion.Euler(40f, -108.8f, 0f); 

        Camera.main.transform.position = desiredPosition;
        Camera.main.transform.rotation = desiredRotation;
    }
    IEnumerator InsertIdCard(GameObject obj)
    {
        float duration = 2.0f;
        Vector3 targetPosition = obj.transform.position - new Vector3(0.097f, 0f, 0.155f); 

        Vector3 startPosition = obj.transform.position;
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            obj.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        obj.transform.position = targetPosition;
    }

    void HandlePasswordActivation()
    { 
        if (!IsPasswordActive)
        {
            IsPasswordActive = true;
            SaveOriginalCameraTransform();
            GameObject closestObject = OutlineSelection.ClosestObject;

            if (closestObject != null && closestObject.CompareTag("SelectablePasswordScreen"))
            {
                MoveCameraAboveObject(closestObject, 0.3f);
                player.GetComponent<PlayerController>().enabled = false;
            }
            else
            {
                ResetCameraPositionAndRotation();
            }
        }
        else
        {
            ResetCameraPositionAndRotation();
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
    void SaveOriginalCameraTransform()
    {
        originalCameraPosition = Camera.main.transform.position;
        originalCameraRotation = Camera.main.transform.rotation;
    }


    void ResetCameraPositionAndRotation()
    {
        Camera.main.transform.position = originalCameraPosition;
        Camera.main.transform.rotation = originalCameraRotation;
        IsPasswordActive = false;
    }

    void MoveCameraAboveObject(GameObject obj, float distanceFactor)
    {
        Vector3 targetPosition = obj.transform.position + Vector3.up * distanceToCamera * distanceFactor;
        Camera.main.transform.position = targetPosition;
        Camera.main.transform.LookAt(obj.transform.position, Vector3.up);

        Vector3 currentRotation = Camera.main.transform.rotation.eulerAngles;
        Camera.main.transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, -(obj.transform.rotation.z + 210f));
    }
    void PlaySound(GameObject obj)
    {
        audioSource = obj.GetComponent<AudioSource>();
        audioSource.Play();
    }
}

