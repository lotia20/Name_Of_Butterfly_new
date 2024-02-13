using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IDCardPickupEvent : MonoBehaviour
{
    float distanceToCamera = 0.8f;
    public GameObject UpperArmL;
    public GameObject HandL;
    public GameObject UpperArmR;
    public GameObject HandR;
    public GameObject player;

    public Image messageImage;

    public AudioSource pickupSound;

    public static bool IdCardPickedUp { get; private set; } = false;

    private Quaternion initialUpperArmLRotation;
    private Quaternion initialHandLRotation;
    private Quaternion initialUpperArmRRotation;
    private Quaternion initialHandRRotation;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (OutlineSelection.IsOutlineEnabled)
            {
                GameObject closestObject = OutlineSelection.ClosestObject;

                if (closestObject != null && closestObject.CompareTag("SelectableIdCard"))
                {
                    player.GetComponent<PlayerController>().enabled = false;
                    StoreInitialArmRotations();
                    MoveObjectToFront(closestObject);
                }
            }
        }
    }
    void StoreInitialArmRotations()
    {
        initialUpperArmLRotation = UpperArmL.transform.rotation;
        initialHandLRotation = HandL.transform.rotation;
        initialUpperArmRRotation = UpperArmR.transform.rotation;
        initialHandRRotation = HandR.transform.rotation;
    }

    void MoveObjectToFront(GameObject obj)
    {
        Vector3 targetPosition = Camera.main.transform.position + Camera.main.transform.forward * distanceToCamera;
        obj.transform.position = targetPosition;

        // ���� �����̼�
        Quaternion localRotation = Quaternion.Euler(0, -180, -180);
        Quaternion targetRotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up) * localRotation;

        obj.transform.rotation = targetRotation;

        StartCoroutine(SequentialArmRotations());
    }
    IEnumerator SequentialArmRotations()
    {
        yield return StartCoroutine(RotateUpperArm(UpperArmL, Vector3.forward, 100f));
        yield return StartCoroutine(RotateUpperArm(UpperArmL, Vector3.right, 80f));
        yield return StartCoroutine(RotateUpperArm(UpperArmL, Vector3.forward, 10f));
        yield return StartCoroutine(RotateHandZ(HandL));
    }

    IEnumerator RotateUpperArm(GameObject arm, Vector3 rotationAxis, float rotationAngle)
    {
        float rotationDuration = 1.0f;
        float elapsedTime = 0f;
        Quaternion startRotation = arm.transform.rotation;

        Quaternion targetRotation = startRotation * Quaternion.Euler(rotationAxis *  rotationAngle);

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            arm.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            yield return null;
        }
    }

    IEnumerator RotateHandZ(GameObject hand)
    {
        float rotationDuration = 1.0f;
        float elapsedTime = 0f;
        Quaternion startRotation = hand.transform.rotation;

        Quaternion targetRotation = startRotation * Quaternion.Euler(0f, 0f, 60f);

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            hand.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            yield return null;
        }
        StartCoroutine(ChangeEmissionColor());
        if (pickupSound != null)
        {
            pickupSound.Play();
        }
    }

    IEnumerator ChangeEmissionColor()
    {
        GameObject displayObject = GameObject.FindGameObjectWithTag("Display");
        if (displayObject != null)
        {
            Renderer renderer = displayObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = renderer.material;
                Color originalColor = material.GetColor("_EmissionColor");
                material.SetColor("_EmissionColor", Color.green);
                yield return new WaitForSeconds(1.0f);
                material.SetColor("_EmissionColor", originalColor);
                DeactivateAndResetArms();
            }
        }
    }
    void DeactivateAndResetArms()
    {
        StartCoroutine(ShowMessageForSeconds(3f));

        if (OutlineSelection.ClosestObject != null)
        {
            OutlineSelection.ClosestObject.SetActive(false);
        }
        ResetArmPositions();
        IdCardPickedUp = true;
    }

    IEnumerator ShowMessageForSeconds(float seconds)
    {
        messageImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(seconds);
        messageImage.gameObject.SetActive(false);
    }
    void ResetArmPositions()
    {
        UpperArmL.transform.rotation = initialUpperArmLRotation;
        HandL.transform.rotation = initialHandLRotation;
        UpperArmR.transform.rotation = initialUpperArmRRotation;
        HandR.transform.rotation = initialHandRRotation;
        player.GetComponent<PlayerController>().enabled = true;
    }
}
