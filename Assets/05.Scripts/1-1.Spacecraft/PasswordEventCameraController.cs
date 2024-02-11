using System.Collections;
using UnityEngine;

public class PasswordEventCameraController : MonoBehaviour
{
    public static bool IsPasswordActive { get; private set; } = false;
    public float distanceToCamera = 2f;
    private bool hasIDCardInserted = false;

    public static Vector3 originalCameraPosition { get; private set; }
    public static Quaternion originalCameraRotation { get; private set; }

    private GameObject idCardObject;

    private void Start()
    {
        idCardObject = GameObject.FindGameObjectWithTag("IdCard");
        if (idCardObject != null)
        {
            idCardObject.SetActive(false); // ���� �� ��Ȱ��ȭ ���·� ����
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!IsPasswordActive && IDCardPickupEvent.IdCardPickedUp && !hasIDCardInserted)
            {
                StartCoroutine(ActivateIDCardSequence(idCardObject));
            }
            if (!IsPasswordActive)
            {
                IsPasswordActive = true;
                originalCameraPosition = Camera.main.transform.position;
                originalCameraRotation = Camera.main.transform.rotation;
                GameObject closestObject = OutlineSelection.ClosestObject;

                if (closestObject != null && closestObject.CompareTag("SelectablePasswordScreen"))
                {
                    MoveCameraAboveObject(closestObject, 0.3f);
                }
                else
                {
                    Camera.main.transform.position = originalCameraPosition;
                    Camera.main.transform.rotation = originalCameraRotation;
                    IsPasswordActive = false;
                }
            }
            else
            {
                Camera.main.transform.position = originalCameraPosition;
                Camera.main.transform.rotation = originalCameraRotation;
                IsPasswordActive = false;
            }
        }
        if (PasswordButtonColorChanger.isBoxOpen)
        {
            IsPasswordActive = false;
        }
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
    IEnumerator ActivateIDCardSequence(GameObject idCardObject)
    {
        // ID ī�� ������Ʈ Ȱ��ȭ
        ActivateIDCard();

        // ID ī�� ����
        InsertIdCard(idCardObject);

        // 2�� ���
        yield return new WaitForSeconds(2f);

        // ID ī�� ������Ʈ ��Ȱ��ȭ
        DeactivateIDCard();

        // ID ī�� ���� �÷��� ����
        hasIDCardInserted = true;
    }

    void InsertIdCard(GameObject obj)
    {
        Vector3 targetPosition = obj.transform.position + new Vector3(-0.12f, 0f, 0f);

        float speed = 0.02f; 
        float step = speed * Time.deltaTime;

        obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPosition, step);
    }


    // Move the camera above the object
    void MoveCameraAboveObject(GameObject obj, float distanceFactor)
    {
        Vector3 targetPosition = obj.transform.position + Vector3.up * distanceToCamera * distanceFactor;
        Camera.main.transform.position = targetPosition;
        Camera.main.transform.LookAt(obj.transform.position, Vector3.up);

        Vector3 currentRotation = Camera.main.transform.rotation.eulerAngles;
        Camera.main.transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, -(obj.transform.rotation.z + 210f)); // �Ǵ� ���ϴ� ������ ����
    }

}

