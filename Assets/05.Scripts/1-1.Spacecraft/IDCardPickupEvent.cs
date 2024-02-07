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

        // 정면 로테이션
        Quaternion localRotation = Quaternion.Euler(0, -180, -180);
        Quaternion targetRotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up) * localRotation;

        obj.transform.rotation = targetRotation;

        StartCoroutine(RotateUpperArmY(UpperArmL));
        StartCoroutine(RotateUpperArmY(UpperArmR, true)); // true: 오른쪽 방향 회전
    }

    IEnumerator RotateUpperArmY(GameObject upperArm, bool reverse = false)
    {
        float rotationDuration = 1.0f;
        float elapsedTime = 0f;
        Quaternion startRotation = upperArm.transform.rotation;

        float yRotation = reverse ? -80f : 80f;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0f, yRotation, 0f);

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            upperArm.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            yield return null;
        }

        StartCoroutine(RotateUpperArmZ(upperArm, reverse));
    }

    IEnumerator RotateUpperArmZ(GameObject upperArm, bool reverse = false)
    {
        float rotationDuration = 1.0f;
        float elapsedTime = 0f;
        Quaternion startRotation = upperArm.transform.rotation;

        float zRotation = reverse ? -5f : 5f;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0f, 0f, zRotation);

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            upperArm.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            yield return null;
        }

        if (reverse)
        {
            StartCoroutine(RotateHandZ(HandR));
        }
        else
        {
            StartCoroutine(RotateHandZ(HandL));
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
                DestroyAndResetArms();
            }
        }
    }
    void DestroyAndResetArms()
    {
        StartCoroutine(ShowMessageForSeconds(3f));
        Destroy(OutlineSelection.ClosestObject);
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
    }
}
