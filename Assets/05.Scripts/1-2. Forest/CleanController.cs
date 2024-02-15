using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanController : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private Texture2D dirtMaskBase;
    [SerializeField] private Texture2D brush;

    [SerializeField] private Material material;

    private Texture2D templateDirtMask;
    public static bool isCleaning{ get; private set; } = false;

    private void Start()
    {
        CreateTexture();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isCleaning = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            isCleaning = false;
        }
        if(isCleaning)
        {
            Clean();
        }
    }

    private void Clean()
    {
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            Vector2 textureCoord = hit.textureCoord;

            int pixelX = (int)(textureCoord.x * templateDirtMask.width);
            int pixelY = (int)(textureCoord.y * templateDirtMask.height);

            for (int x = 0; x < brush.width; x++)
            {
                for (int y = 0; y < brush.height; y++)
                {
                    Color pixelDirt = brush.GetPixel(x, y);
                    Color pixelDirtMask = templateDirtMask.GetPixel(pixelX + x, pixelY + y);

                    templateDirtMask.SetPixel(pixelX + x,
                        pixelY + y,
                        new Color(0, pixelDirtMask.g * pixelDirt.g, 0));
                }
            }
            templateDirtMask.Apply();
        }
    }

    private void CreateTexture()
    {
        templateDirtMask = new Texture2D(dirtMaskBase.width, dirtMaskBase.height);
        templateDirtMask.SetPixels(dirtMaskBase.GetPixels());
        templateDirtMask.Apply();

        material.SetTexture("_MaskTexture", templateDirtMask);
    }
}