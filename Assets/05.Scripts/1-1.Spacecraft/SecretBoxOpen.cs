using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretBoxOpen : MonoBehaviour
{
    public Transform upperbox;
    [SerializeField] private float openAngle;
    [SerializeField] private float openSpeed;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool hasRestoredCamera = false;

    void Start()
    {
        closedRotation = upperbox.transform.rotation;
        openRotation = Quaternion.Euler(openAngle, 33f, 0f);
    }
    void Update()
    {
        if (PasswordButtonColorChanger.isBoxOpen && !hasRestoredCamera)
        {
            StartCoroutine(OpenBox());

            if (!hasRestoredCamera)
            {
                RestoreCameraPositionAndRotation();
                hasRestoredCamera = true;
            }
        }
    }
    IEnumerator OpenBox()
    {
        Debug.Log("open");
        float elapsedTime = 0f;
        while (elapsedTime < openSpeed)
        {
            upperbox.transform.rotation = Quaternion.Lerp(closedRotation, openRotation, elapsedTime / openSpeed);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        upperbox.transform.rotation = openRotation;
    }

    void RestoreCameraPositionAndRotation()
    {
        if (Camera.main != null)
        {
            Camera.main.transform.position = PasswordEventCameraController.originalCameraPosition;
            Camera.main.transform.rotation = PasswordEventCameraController.originalCameraRotation;
        }
    }
}