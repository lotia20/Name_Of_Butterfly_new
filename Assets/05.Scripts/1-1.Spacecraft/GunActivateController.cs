using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunActiveController : MonoBehaviour
{
    public GameObject deactivateGun;
    public GameObject activateGun;
    public GameObject fullGauge;

    public static bool isGunActivate{ get; private set; } = false;

    

    void Start()
    {
        activateGun.SetActive(false);
        deactivateGun.SetActive(true); 
        fullGauge.SetActive(false);
    }
    void Update()
    {
        if (PasswordButtonColorChanger.isBoxOpen)
        {
            Debug.Log("gun Activate");
            GunActivate(); 
        }
    }

    void GunActivate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            activateGun.SetActive(true);
            deactivateGun.SetActive(false);
            fullGauge.SetActive(true);
            isGunActivate = true;
        }
    }

}
