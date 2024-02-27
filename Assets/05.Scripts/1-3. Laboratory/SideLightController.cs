using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideLightController : MonoBehaviour
{
    public GameObject sideDoorLight;
    public float emissionIncreaseSpeed = 1f;
    public float targetEmissionIntensity = 6f; 

    private Renderer lightRenderer;
    private bool isIncreasingEmission = false;

    void Start()
    {
        lightRenderer = sideDoorLight.GetComponent<Renderer>();
        SetEmissionIntensity(0f);
    }

    void Update()
    {
        if (MiddleDoorOpener.isDoorOpen && !IsEmissionAtTarget())
        {
            isIncreasingEmission = true;
            IncreaseEmission();
        }
        else
        {
            isIncreasingEmission = false;
        }
    }

    void IncreaseEmission()
    {
        float newEmissionIntensity = lightRenderer.material.GetColor("_EmissionColor").r + emissionIncreaseSpeed * Time.deltaTime;
        SetEmissionIntensity(newEmissionIntensity);
    }

    void SetEmissionIntensity(float intensity)
    {
        Color newEmissionColor = new Color(intensity, intensity, intensity);
        lightRenderer.material.SetColor("_EmissionColor", newEmissionColor);
    }

    bool IsEmissionAtTarget()
    {
        return lightRenderer.material.GetColor("_EmissionColor").r >= targetEmissionIntensity;
    }
}

