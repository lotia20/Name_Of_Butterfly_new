using System.Collections;
using UnityEngine;

public class MiddleDoorOpener : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float openSpeed;
    [SerializeField] private float cameraMoveSpeed = 10f;
    [SerializeField] private float cameraDistanceFromObject = 3f;

    public GameObject idCardObject;
    public GameObject door;
    public GameObject idDisplay;
    public GameObject UpDoor;
    public GameObject DownDoor;
    public GameObject IDdevice;
    public GameObject gun;

    public GameObject upperArmR;
    public GameObject handR;

    public float interactionDistance = 10f;

    private AudioSource audioSource;
    private bool soundPlayed = false;

    private ObjectHighlighter objectHighlighter;

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;


    public static bool isDoorOpen { get; private set; } = false;

    private void Start()
    {
        objectHighlighter = GetComponent<ObjectHighlighter>();
        if (idCardObject != null)
        {
            idCardObject.SetActive(false);
        }
    }
    void Update()
    {
        objectHighlighter.UpdateOutline(IDdevice);
        if(ObjectHighlighter.IsHighlightOn && Input.GetKeyDown(KeyCode.E))
        {
            player.GetComponent<PlayerController>().enabled = false;
            gun.SetActive(false);
            StartCoroutine(SequentialDoorOpen(IDdevice));
        }
    }
    IEnumerator SequentialDoorOpen(GameObject obj)
    {
        Vector3 targetPosition = new Vector3(-2.8f, 9.68f, 164.31f);
        Quaternion targetRotation = Quaternion.Euler(2.554f, -177.188f, -0.549f);
        yield return StartCoroutine(MoveCameraToLookAtObject(targetPosition, targetRotation));
        idCardObject.SetActive(true);
        yield return StartCoroutine(RotateUpperArm(upperArmR, Vector3.up, -45f));
        yield return StartCoroutine(RotateUpperArm(handR, Vector3.forward, 10f));
        yield return new WaitForSeconds(2f);
        TurnOnIdLight();
        PlaySound(idCardObject);
        yield return new WaitForSeconds(1f);
        idCardObject.SetActive(false);
        yield return StartCoroutine(MoveCameraToLookAtObject(originalCameraPosition, originalCameraRotation));
        yield return StartCoroutine(MoveCameraToLookAtObject(UpDoor.transform));
        isDoorOpen = true;
        yield return StartCoroutine(OpenDoor());
        yield return StartCoroutine(MoveCameraToLookAtObject(originalCameraPosition, originalCameraRotation));
        player.GetComponent<PlayerController>().enabled = true;
        gun.SetActive(true);
    }
    IEnumerator MoveCameraToLookAtObject(Vector3 targetPosition, Quaternion targetRotation)
    {
        SaveOriginalCameraTransform();
        float duration = 2.0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            Camera.main.transform.position = Vector3.Lerp(originalCameraPosition, targetPosition, t);
            Camera.main.transform.rotation = Quaternion.Slerp(originalCameraRotation, targetRotation, t);
            yield return null;
        }

        Camera.main.transform.position = targetPosition;
        Camera.main.transform.rotation = targetRotation;
    }
    IEnumerator RotateUpperArm(GameObject arm, Vector3 rotationAxis, float rotationAngle)
    {
        float rotationDuration = 1.0f;
        float elapsedTime = 0f;
        Quaternion startRotation = arm.transform.rotation;

        Quaternion targetRotation = startRotation * Quaternion.Euler(rotationAxis * rotationAngle);

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            arm.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            yield return null;
        }
    }
    void SaveOriginalCameraTransform()
    {
        originalCameraPosition = Camera.main.transform.position;
        originalCameraRotation = Camera.main.transform.rotation;
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
    void TurnOnIdLight()
    {
        Renderer idLightRenderer = idDisplay.GetComponent<Renderer>();
        if (idLightRenderer != null)
        {
            idLightRenderer.material.SetColor("_EmissionColor", Color.cyan);

            float intensity = 25f;
            Color emissionColor = Color.cyan * intensity;
            idLightRenderer.material.SetColor("_EmissionColor", emissionColor);
        }
    }
    IEnumerator OpenDoor()
    {
        if (!soundPlayed)
        {
            PlaySound(door);
            soundPlayed = true;
        }

        float targetUpDoorPositionY = UpDoor.transform.position.y + 10f; 
        float targetDownDoorPositionY = DownDoor.transform.position.y - 10f; 

        while (UpDoor.transform.position.y < targetUpDoorPositionY && DownDoor.transform.position.y > targetDownDoorPositionY)
        {
            UpDoor.transform.Translate(Vector3.up * Time.deltaTime * openSpeed, Space.World);
            DownDoor.transform.Translate(Vector3.down * Time.deltaTime * openSpeed, Space.World);
            yield return null; 
        }
    }
    void PlaySound(GameObject obj)
    {
        audioSource = obj.GetComponent<AudioSource>();
        audioSource.Play();
    }
}
