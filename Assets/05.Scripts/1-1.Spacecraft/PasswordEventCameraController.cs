using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordEventCameraController : MonoBehaviour
{
    public static bool IsPasswordActive { get; private set; } = false;
    public float distanceToCamera = 2f;
    public static Vector3 originalCameraPosition { get; private set; }
    public static Quaternion originalCameraRotation { get; private set; }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
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


    // Move the camera above the object
    void MoveCameraAboveObject(GameObject obj, float distanceFactor)
    {
        Vector3 targetPosition = obj.transform.position + Vector3.up * distanceToCamera * distanceFactor;
        Camera.main.transform.position = targetPosition;
        Camera.main.transform.LookAt(obj.transform.position, Vector3.up);

        Vector3 currentRotation = Camera.main.transform.rotation.eulerAngles;
        Camera.main.transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, -(obj.transform.rotation.z + 210f)); // 또는 원하는 각도로 수정
    }

}
