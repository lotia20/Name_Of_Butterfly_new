using System.Collections;
using UnityEngine;

public class CleanController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    // [SerializeField] private Texture2D dirtMaskBase;
    // [SerializeField] private Texture2D brush;
    // [SerializeField] private Material material;
    [SerializeField] private ParticleSystem washParticles; // Particle System 추가

    // private Texture2D templateDirtMask;
    public static bool isCleaning{ get; private set; } = false;
    public static bool isDecreasing{ get; set; } = false;
    public GunChargeController gunChargeControllerInstance;

    //마우스 범위 조절 
    private int minX;
    private int maxX;
    private int minY;
    private int maxY;


    private void Start()
    {
        gunChargeControllerInstance = FindObjectOfType<GunChargeController>();
        // CreateTexture();
    }

    private void Update()
    {
        UpdateMouseCoordinates();

        Vector3 mousePosition = Input.mousePosition;
        bool isInCleaningArea = mousePosition.x >= minX && mousePosition.x <= maxX && mousePosition.y >= minY && mousePosition.y <= maxY;
        
        if(gunChargeControllerInstance.gaugeIndex != 0)
        { 
            if (Input.GetMouseButtonDown(0) && isInCleaningArea)
            {
                isCleaning = true;
                // 청소 중일 때 Particle System 활성화
                washParticles.Play();
            }
            if (Input.GetMouseButtonUp(0))
            {
                isCleaning = false;
                //청소 중이 아닐 때 Particle System 비활성화
                if(isDecreasing)
                {
                    gunChargeControllerInstance.gaugeIndex -= 1;
                    Debug.Log(gunChargeControllerInstance.gaugeIndex);
                    gunChargeControllerInstance.DecreaseGauge(gunChargeControllerInstance.gaugeIndex);
                    isDecreasing = false;
                }
                washParticles.Stop();
            }
            if (isCleaning)
            {
                Clean();
                isDecreasing = true;
            }
        }
    }

    private void Clean()
    {
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            Debug.Log("Clean");
            // Vector2 textureCoord = hit.textureCoord;

            // int pixelX = (int)(textureCoord.x * templateDirtMask.width);
            // int pixelY = (int)(textureCoord.y * templateDirtMask.height);

            // for (int x = 0; x < brush.width; x++)
            // {
            //     for (int y = 0; y < brush.height; y++)
            //     {
            //         Color pixelDirt = brush.GetPixel(x, y);
            //         Color pixelDirtMask = templateDirtMask.GetPixel(pixelX + x, pixelY + y);

            //         templateDirtMask.SetPixel(pixelX + x,
            //             pixelY + y,
            //             new Color(0, pixelDirtMask.g * pixelDirt.g, 0));
            //     }
            // }
            // templateDirtMask.Apply();
        }
    }

    // private void CreateTexture()
    // {
    //     templateDirtMask = new Texture2D(dirtMaskBase.width, dirtMaskBase.height);
    //     templateDirtMask.SetPixels(dirtMaskBase.GetPixels());
    //     templateDirtMask.Apply();

    //     material.SetTexture("_MaskTexture", templateDirtMask);
    // }

    private void UpdateMouseCoordinates()
    {
        minX = (int)(0.375f * Screen.width); // 예시: 화면 가로 크기의 75%
        maxX = (int)(0.6f * Screen.width);
        minY = 0;
        maxY = Screen.height;
    }
}
