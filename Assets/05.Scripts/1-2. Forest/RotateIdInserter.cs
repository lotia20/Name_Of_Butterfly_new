using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RotateIdInserter : MonoBehaviour
{
    public static bool IsRockRotated { get; private set; } = false;
    //추후 enable 넣기 
    //public CleaningScript cleaningScript;
    public bool cleanComplete = true;
    [SerializeField] private float interactDistance;
    public GameObject idInserter;
    public GameObject player;
    public CampingCarHighlighter campingCarHighlighter;

    private GameObject idCardObject;
    private bool IsDoorOpened = false;

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;


    private void Start()
    {
        campingCarHighlighter = GetComponent<CampingCarHighlighter>();
        idCardObject = GameObject.FindGameObjectWithTag("SelectableIdCard");
        if (idCardObject != null)
        {
            idCardObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (CanInteract() && Input.GetKeyDown(KeyCode.E) && !IsRockRotated)
        {
            player.GetComponent<PlayerController>().enabled = false;
            StartCoroutine(SequentialRockRotation(idInserter));
        }
        if (IsRockRotated)
        {
            campingCarHighlighter.UpdateOutline(idInserter);
            if (CanInteract() && Input.GetKeyDown(KeyCode.E) && !IsDoorOpened)
            {
                player.GetComponent<PlayerController>().enabled = false;
                StartCoroutine(ActivateIDCardSequence(idCardObject));
            }
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
        IsRockRotated = true;
    }
    IEnumerator ActivateIDCardSequence(GameObject idCardObject)
    {
        Vector3 targetPosition = new Vector3(-453.146f, 3.087f, 103.19f);
        Quaternion targetRotation = Quaternion.Euler(5.268f, 213.531f, -4.972f);
        yield return StartCoroutine(MoveCameraToSide(targetPosition, targetRotation));
        ActivateIDCard();
        yield return StartCoroutine(InsertIDCard(idCardObject));
        yield return new WaitForSeconds(3f);
        RestoreOriginalCameraTransform();
        DeactivateIDCard();
        yield return StartCoroutine(ShakeAndDisappearRock());
        player.GetComponent<PlayerController>().enabled = true;
        IsDoorOpened = true;
    }
    void ActivateIDCard()
    {
        if (idCardObject != null)
        {
            idCardObject.SetActive(true);
        }
    }
    void DeactivateIDCard()
    {
        if (idCardObject != null)
        {
            idCardObject.SetActive(false);
        }
    }

    IEnumerator InsertIDCard(GameObject idCardObject)
    {
        Vector3 initialPosition = idCardObject.transform.position;
        Vector3 targetPosition = new Vector3(-453.5987f, 2.926783f, 102.4073f);
        float elapsedTime = 0f;
        float moveSpeed = 0.4f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * moveSpeed;
            idCardObject.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime);
            yield return null;
        }
    }
    IEnumerator ShakeAndDisappearRock()
    {
        GameObject shakingRock = GameObject.FindGameObjectWithTag("ShakingRock");
        yield return StartCoroutine(MoveCameraToLookAtObject(shakingRock.transform));
        StartCoroutine(ShakeAndDisappearRock(shakingRock));
        yield return new WaitForSeconds(5f);
        yield return StartCoroutine(ResetCameraPositionAndRotation());
    }
    IEnumerator MoveCameraToSide(Vector3 targetPosition, Quaternion targetRotation)
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

    IEnumerator ShakeAndDisappearRock(GameObject rock)
    {
        float shakeSpeed = 20f; // 흔들림 속도
        float disappearSpeed = 0.2f; // 사라짐 속도
        float shakeRange = 1f; // 흔들림 범위

        Vector3 startPosition = rock.transform.position;

        while (rock.activeSelf && rock.transform.localScale.y > 0)
        {
            // 흔들림 효과 적용
            float shakeOffsetX = UnityEngine.Random.Range(-shakeRange, shakeRange);
            float shakeOffsetZ = UnityEngine.Random.Range(-shakeRange, shakeRange);
            Vector3 shakeOffset = new Vector3(shakeOffsetX, 0, shakeOffsetZ);
            rock.transform.position = startPosition + shakeOffset;

            // Y축으로 감소
            Vector3 newPosition = rock.transform.position;
            newPosition.y -= disappearSpeed * Time.deltaTime;
            rock.transform.position = newPosition;

            yield return new WaitForSeconds(shakeSpeed * Time.deltaTime);
        }

        // GameObject를 비활성화하여 사라지게 함
        rock.SetActive(false);
    }


    void RestoreOriginalCameraTransform()
    {
        StartCoroutine(MoveCameraToSide(originalCameraPosition, originalCameraRotation));
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
}

