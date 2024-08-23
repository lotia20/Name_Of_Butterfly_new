using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class End : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float openSpeed;
    [SerializeField] private float triggerDistance = 5f;
    [SerializeField] private CinemachineVirtualCamera vcam; // Cinemachine Virtual Camera
    [SerializeField] private Camera mainCamera; // ���� ī�޶�
    [SerializeField] private Camera secondaryCamera; // �� ��° ī�޶�
    [SerializeField] private CinemachineDollyCart dollyCart; // Dolly Cart
    [SerializeField] private CanvasGroup panel1CanvasGroup; // ù ��° �г��� CanvasGroup
    [SerializeField] private CanvasGroup panel2CanvasGroup; // �� ��° �г��� CanvasGroup
    [SerializeField] private float panelFadeDuration = 3f; // �г� ���̵� ��/�ƿ� ���� �ð�
    [SerializeField] private float cinemachineDuration = 30f; // �ó׸ӽ� ī�޶� ���� ���� �ð�

    FadeEffect fade;
    public GameObject door;
    public GameObject UpDoor;
    public GameObject DownDoor;

    private AudioSource audioSource;
    private bool soundPlayed = false;
    private bool isTriggered = false;

    void Start()
    {
        fade = FindObjectOfType<FadeEffect>();
        vcam.gameObject.SetActive(false); // �ʱ⿡�� �ó׸ӽ� ī�޶� ��Ȱ��ȭ
        secondaryCamera.gameObject.SetActive(false); // �� ��° ī�޶� ��Ȱ��ȭ
        dollyCart.gameObject.SetActive(false); // Dolly Cart ��Ȱ��ȭ
        panel1CanvasGroup.alpha = 0f; // ù ��° �г� �ʱ� ��Ȱ��ȭ
        panel2CanvasGroup.alpha = 0f; // �� ��° �г� �ʱ� ��Ȱ��ȭ
    }

    void Update()
    {
        if (!isTriggered && Vector3.Distance(transform.position, door.transform.position) <= triggerDistance)
        {
            isTriggered = true;
            StartCoroutine(HandleDoorAndSceneTransition());
        }
    }

    private IEnumerator HandleDoorAndSceneTransition()
    {
        // 1. ���̵� �� (ȭ�� ��ο���)
        fade.FadeIn();
        yield return new WaitUntil(() => !fade.isFadingIn);

        // 2. ���� ī�޶� ��Ȱ��ȭ, �� ��° ī�޶� �� �ó׸ӽ� ī�޶� Ȱ��ȭ
        mainCamera.gameObject.SetActive(false);
        secondaryCamera.gameObject.SetActive(true);
        vcam.gameObject.SetActive(true);

        player.GetComponent<PlayerController>().enabled = false;

        // 3. ���̵� �ƿ� (ȭ�� �����)
        fade.FadeOut();
        yield return new WaitUntil(() => !fade.isFadingOut);

        // 4. Dolly Cart Ȱ��ȭ �� �̵�
        yield return StartCoroutine(OpenDoor());
        dollyCart.gameObject.SetActive(true); // Dolly Cart Ȱ��ȭ
        yield return new WaitForSeconds(cinemachineDuration); // �ó׸ӽ� ī�޶� ���� ���� �ð� ���

        fade.FadeIn();
        // 5. �г� ��ȯ �� �� ��ȯ ó��
        yield return StartCoroutine(HandlePanelTransition());
    }

    private IEnumerator HandlePanelTransition()
    {
        // ù ��° �г� ó��
        yield return StartCoroutine(FadePanel(panel1CanvasGroup));

        // �� ��° �г� ó��
        yield return StartCoroutine(FadePanel(panel2CanvasGroup));

        // 3�� ��� �� �� ��ȯ
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("0.Start"); // �����Ϸ��� �� �̸����� ��ü
    }

    private IEnumerator FadePanel(CanvasGroup canvasGroup)
    {
        // �г� ���̵� �� (���� ����)
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 1f, panelFadeDuration));

        // �г� 1�� ���� ���� (�ʿ信 ���� ���� ����)
        yield return new WaitForSeconds(1f);

        // �г� ���̵� �ƿ� (���� ����)
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 1f, 0f, panelFadeDuration));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float start, float end, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = end;
    }

    IEnumerator OpenDoor()
    {
        if (!soundPlayed)
        {
            PlaySound(door);
            soundPlayed = true;
        }

        float targetUpDoorPositionY = UpDoor.transform.position.y + 10f;
        float targetDownDoorPositionY = DownDoor.transform.position.y - 10f;

        while (UpDoor.transform.position.y < targetUpDoorPositionY && DownDoor.transform.position.y > targetDownDoorPositionY)
        {
            UpDoor.transform.Translate(Vector3.up * Time.deltaTime * openSpeed, Space.World);
            DownDoor.transform.Translate(Vector3.down * Time.deltaTime * openSpeed, Space.World);
            yield return null;
        }
    }

    void PlaySound(GameObject obj)
    {
        audioSource = obj.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource not found on the door object.");
        }
    }
}


