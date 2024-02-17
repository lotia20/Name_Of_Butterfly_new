using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleDoorOpener : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float openSpeed;

    public GameObject door;
    public GameObject doorLight;
    public GameObject sideDoorLight;
    public GameObject UpDoor;
    public GameObject DownDoor;
    public float interactionDistance = 10f;

    public static bool isDoorOpen { get; private set; } = false;

    void Update()
    {
        if (IsPlayerNearDoor())
        {
            OpenDoor();
            StartCoroutine(TurnOnDoorLight(doorLight));
            //StartCoroutine(TurnOnDoorLight(sideDoorLight));
            isDoorOpen = true;
        }
    }

    bool IsPlayerNearDoor()
    {
        float distanceToPlayer = Vector3.Distance(door.transform.position, player.position);
        return distanceToPlayer <= interactionDistance;
    }

    void OpenDoor()
    {
        if (UpDoor.transform.position.y <= 10f)
            UpDoor.transform.Translate(Vector3.up * Time.deltaTime * openSpeed, Space.World);
        if (DownDoor.transform.position.y >= -1f)
            DownDoor.transform.Translate(Vector3.down * Time.deltaTime * openSpeed, Space.World);
    }

    IEnumerator TurnOnDoorLight(GameObject obj)
    {
        Renderer objRenderer = obj.GetComponent<Renderer>();
        if (objRenderer != null)
        {

            Color targetColor = objRenderer.material.GetColor("_EmissionColor");
            float targetIntensity = 40f;

            float currentIntensity = 3f;

            float elapsedTime = 0f;
            while (elapsedTime < 7f)
            {

                float t = elapsedTime / 7f;
                float intensity = Mathf.Lerp(currentIntensity, targetIntensity, Mathf.Sqrt(t)); 
                Color newColor = targetColor * intensity;

                objRenderer.material.SetColor("_EmissionColor", newColor);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            objRenderer.material.SetColor("_EmissionColor", targetColor * targetIntensity);
        }     
    }
}



