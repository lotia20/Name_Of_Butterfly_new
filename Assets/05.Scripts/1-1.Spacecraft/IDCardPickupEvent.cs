using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IDCardPickupEvent : MonoBehaviour
{
    float distanceToCamera = 0.45f;
    public GameObject UpperArmL;
    public GameObject HandL;
    public GameObject FingerL2;
    public GameObject FingerL3;
    public GameObject FingerL4;
    public GameObject player;

    public Image messageImage;

    public AudioSource pickupSound;
    public static bool IdCardPickedUp { get; private set; } = false;

    private Quaternion initialUpperArmLRotation;
    private Quaternion initialHandLRotation;
    private Quaternion initialFingerL2Rotation;
    private Quaternion initialFingerL3Rotation;
    private Quaternion initialFingerL4Rotation;

    private OutlineSelection outlineSelectionScript;

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
        initialFingerL2Rotation = FingerL2.transform.rotation;
        initialFingerL3Rotation = FingerL3.transform.rotation;
        initialFingerL4Rotation = FingerL4.transform.rotation;
    }

    void MoveObjectToFront(GameObject obj)
    {
        Vector3 targetPosition = Camera.main.transform.position + Camera.main.transform.forward * distanceToCamera;
        obj.transform.position = targetPosition;

        // 정면 로테이션
        Quaternion localRotation = Quaternion.Euler(0, -180, -180);
        Quaternion targetRotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up) * localRotation;

        obj.transform.rotation = targetRotation;

        StartCoroutine(SequentialArmRotations(obj));
    }

    IEnumerator SequentialArmRotations(GameObject obj)
    {
        float objectHeight = obj.transform.position.y;
        float rotationFactor = 74f;
        float targetRotationAngle = objectHeight * rotationFactor;
        yield return StartCoroutine(RotateUpperArm(UpperArmL, Vector3.forward, targetRotationAngle));
        yield return StartCoroutine(RotateUpperArm(UpperArmL, Vector3.right, 80f));
        yield return StartCoroutine(RotateUpperArm(UpperArmL, Vector3.forward, 19.2f));
        yield return StartCoroutine(RotateHandZ(HandL));
        yield return StartCoroutine(RotateFingers(FingerL2, FingerL3, FingerL4));
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
    }

    IEnumerator RotateFingers(GameObject finger2, GameObject finger3, GameObject finger4)
    {
        float rotationDuration = 1.0f;
        float elapsedTime = 0f;
        Quaternion startRotation2 = finger2.transform.rotation;
        Quaternion startRotation3 = finger3.transform.rotation;
        Quaternion startRotation4 = finger4.transform.rotation;

        Quaternion targetRotation = startRotation2 * Quaternion.Euler(0f, 0f, -60f); // Adjust the target rotation as needed

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            finger2.transform.rotation = Quaternion.Slerp(startRotation2, targetRotation, elapsedTime / rotationDuration);
            finger3.transform.rotation = Quaternion.Slerp(startRotation3, targetRotation, elapsedTime / rotationDuration);
            finger4.transform.rotation = Quaternion.Slerp(startRotation4, targetRotation, elapsedTime / rotationDuration);
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
        //추후 넣을지 안넣을지 결정할거임
        //StartCoroutine(ShowMessageForSeconds(3f));

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
        FingerL2.transform.rotation = initialFingerL2Rotation;
        FingerL3.transform.rotation = initialFingerL3Rotation;
        FingerL4.transform.rotation = initialFingerL4Rotation;
        player.GetComponent<PlayerController>().enabled = true;
    }
}
