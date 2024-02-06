using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject doorLight;
    public GameObject doorU;
    public GameObject doorD;

    Vector3 doorLightOriginScale;
    Vector3 doorUClosePosition;
    Vector3 doorDClosePosition;
    
    [SerializeField] private float openSpeed;
    
    void Start()
    {
        doorLightOriginScale = doorLight.transform.localScale;
        doorUClosePosition = doorU.transform.position;
        doorDClosePosition = doorD.transform.position;
        
    }

    void Update()
    {
         if(GunActiveController.isGunActivate)
         {
            DoorOpen();
            StartCoroutine(EmitDoorLight());
         }
    }

    void DoorOpen()
    {
        if(doorU.transform.position.y <= 3.0f )
            doorU.transform.Translate(Vector3.up * Time.deltaTime * openSpeed);
        if(doorD.transform.position.y >= -2.5f)
            doorD.transform.Translate(Vector3.down * Time.deltaTime * openSpeed);
    }
    
    void DoorClose()
    {
        doorU.transform.position = Vector3.MoveTowards(doorU.transform.position, doorUClosePosition, 0.01f);
        doorD.transform.position = Vector3.MoveTowards(doorD.transform.position, doorDClosePosition, 0.01f);
    }

    IEnumerator EmitDoorLight()
    {
        float elapsedTime = 0f;
        float duration = 2.0f;

        while(elapsedTime < duration)
        {
            float newScale = doorLightOriginScale.x + 77f;
            doorLight.transform.localScale = new Vector3(newScale, doorLightOriginScale.y, doorLightOriginScale.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
    }
}
