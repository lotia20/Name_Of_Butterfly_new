using System.Collections;
using TMPro;
using UnityEngine;

public class PasswordButtonColorChanger : MonoBehaviour
{
    private MaterialPropertyBlock propertyBlock;
    private Color originalColor;
    private float originalIntensity;
    private bool isChangingColor = false;
    public static bool isBoxOpen { get; private set; } = false;

    public TextMeshPro passwordTextBox;
    public AudioClip successSound;

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
        if (PasswordEventCameraController.IsPasswordActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("PasswordButton"))
                {
                    Debug.Log("Clicked on password button");

                    int buttonNumber;
                    if (int.TryParse(hit.collider.gameObject.name.Substring("Upper box button.".Length), out buttonNumber))
                    {
                        // 클릭 시 색상 -> 논의 사항 ( white로 해놓음 )
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
                                }
                                else
                                {
                                    StartCoroutine(ShowErrorAndReset());
                                }
                            }
                            else
                            {
                                passwordTextBox.text += buttonNumber.ToString();
                            }
                        }
                    }
                }
            }
        }
    }

    void ChangeColor(GameObject obj, Color targetColor, float targetIntensity, float duration)
    {
        isChangingColor = true;

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
        isChangingColor = false;

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
            // "error!!"를 1초 동안 표시 -> 색상 논의사항
            passwordTextBox.text = "<color=red><size=0.006>error!!</size></color>";
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
}
