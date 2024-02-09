using System.Collections;
using TMPro;
using UnityEngine;

public class PasswordButtonColorChanger : MonoBehaviour
{
    private MaterialPropertyBlock propertyBlock;
    private Color originalColor;
    private float originalIntensity;
    private bool hasIDCardInserted = false;
    public static bool isBoxOpen { get; private set; } = false;

    public TextMeshPro passwordTextBox;
    public AudioClip successSound;
    public AudioClip clickSound;

    private AudioSource audioSource;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
        originalIntensity = renderer.material.GetColor("_EmissionColor").r;
        propertyBlock = new MaterialPropertyBlock();

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (PasswordEventCameraController.IsPasswordActive && IDCardPickupEvent.IdCardPickedUp && !hasIDCardInserted)
        {
            ActivateSelectableIDCard();
            // 모션 함수 
            hasIDCardInserted = true;
        }

        if (PasswordEventCameraController.IsPasswordActive && IDCardPickupEvent.IdCardPickedUp)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("PasswordButton"))
                {
                    int buttonNumber;
                    if (int.TryParse(hit.collider.gameObject.name.Substring("Upper box button.".Length), out buttonNumber))
                    {
                        ChangeColor(hit.collider.gameObject, Color.white, 1.0f, 0.2f);
                        if (passwordTextBox != null)
                        {
                            if (buttonNumber == 10)
                            {
                                passwordTextBox.text += "0";
                            }
                            else if (buttonNumber == 11)
                            {
                                if (passwordTextBox.text.Length > 0)
                                {
                                    passwordTextBox.text = passwordTextBox.text.Substring(0, passwordTextBox.text.Length - 1);
                                }
                            }
                            else if (buttonNumber == 12)
                            {
                                if (passwordTextBox.text == "12341234")
                                {
                                    PlaySound(successSound);
                                    StartCoroutine(WaitForSecondsAndReset(2.0f));
                                    DisableOutlineAndPasswordScripts();
                                    
                                }
                                else
                                {
                                    StartCoroutine(ShowErrorAndReset());
                                }
                            }
                            else
                            {
                                if (passwordTextBox.text.Length > 15)
                                {
                                    StartCoroutine(ShowErrorAndReset());
                                }
                                else
                                {
                                    passwordTextBox.text += buttonNumber.ToString();
                                    PlaySound(clickSound);
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            passwordTextBox.text = "<color=red><size=0.006>Inactive</size></color>";
            if (!Input.GetMouseButtonDown(0))
            {
                passwordTextBox.text = "";
            }
        }
    }

    void ActivateSelectableIDCard()
    {
        GameObject idCardObject = GameObject.FindGameObjectWithTag("SelectableIDCard");
        if (idCardObject != null)
        {
            idCardObject.SetActive(true);
        }
    }

    void ChangeColor(GameObject obj, Color targetColor, float targetIntensity, float duration)
    {

        Renderer renderer = obj.GetComponent<Renderer>();
        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_EmissionColor", targetColor);
        Color currentEmissionColor = propertyBlock.GetColor("_EmissionColor");
        propertyBlock.SetColor("_EmissionColor", new Color(targetIntensity, targetIntensity, targetIntensity, currentEmissionColor.a));

        renderer.SetPropertyBlock(propertyBlock);

        StartCoroutine(ResetColor(obj, originalColor, originalIntensity, duration));
    }

    IEnumerator ResetColor(GameObject obj, Color targetColor, float targetIntensity, float duration)
    {
        yield return new WaitForSeconds(duration);

        Renderer renderer = obj.GetComponent<Renderer>();
        renderer.GetPropertyBlock(propertyBlock);

        propertyBlock.SetColor("_EmissionColor", targetColor);
        Color currentEmissionColor = propertyBlock.GetColor("_EmissionColor");
        propertyBlock.SetColor("_EmissionColor", new Color(targetIntensity, targetIntensity, targetIntensity, currentEmissionColor.a));

        renderer.SetPropertyBlock(propertyBlock);
    }

    IEnumerator WaitForSecondsAndReset(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isBoxOpen = true;
        passwordTextBox.text = "";
    }

    IEnumerator ShowErrorAndReset()
    {
        if (passwordTextBox != null)
        {
            passwordTextBox.text = "<color=red><size=0.006>XXXX</size></color>";
            yield return new WaitForSeconds(1.0f);

            passwordTextBox.text = "";
        }
    }


    void PlaySound(AudioClip sound)
    {
        if (sound != null && audioSource != null)
        {
            audioSource.clip = sound;
            audioSource.Play();
        }
    }

    private void DisableOutlineAndPasswordScripts()
    {
        GameObject hbdCardObject = GameObject.Find("HBDcard");
        if (hbdCardObject != null)
        {
            Destroy(hbdCardObject);
        }

        GameObject outlineObject = GameObject.FindWithTag("OutlineObject");
        if (outlineObject != null)
        {
            OutlineSelection outlineScript = outlineObject.GetComponent<OutlineSelection>();
            if (outlineScript != null)
            {
                outlineScript.enabled = false;
            }
        }

        GameObject passwordObject = GameObject.FindWithTag("SelectablePasswordScreen");
        if (passwordObject != null)
        {
            PasswordEventCameraController passwordScript = passwordObject.GetComponent<PasswordEventCameraController>();
            if (passwordScript != null)
            {
                passwordScript.enabled = false;
            }
        }
    }
}