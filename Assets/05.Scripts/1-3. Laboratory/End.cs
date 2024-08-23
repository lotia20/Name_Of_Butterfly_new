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
    [SerializeField] private Camera mainCamera; // 메인 카메라
    [SerializeField] private Camera secondaryCamera; // 두 번째 카메라
    [SerializeField] private CinemachineDollyCart dollyCart; // Dolly Cart
    [SerializeField] private CanvasGroup panel1CanvasGroup; // 첫 번째 패널의 CanvasGroup
    [SerializeField] private CanvasGroup panel2CanvasGroup; // 두 번째 패널의 CanvasGroup
    [SerializeField] private float panelFadeDuration = 3f; // 패널 페이드 인/아웃 지속 시간
    [SerializeField] private float cinemachineDuration = 30f; // 시네머신 카메라 연출 지속 시간

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
        vcam.gameObject.SetActive(false); // 초기에는 시네머신 카메라 비활성화
        secondaryCamera.gameObject.SetActive(false); // 두 번째 카메라도 비활성화
        dollyCart.gameObject.SetActive(false); // Dolly Cart 비활성화
        panel1CanvasGroup.alpha = 0f; // 첫 번째 패널 초기 비활성화
        panel2CanvasGroup.alpha = 0f; // 두 번째 패널 초기 비활성화
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
        // 1. 페이드 인 (화면 어두워짐)
        fade.FadeIn();
        yield return new WaitUntil(() => !fade.isFadingIn);

        // 2. 메인 카메라 비활성화, 두 번째 카메라 및 시네머신 카메라 활성화
        mainCamera.gameObject.SetActive(false);
        secondaryCamera.gameObject.SetActive(true);
        vcam.gameObject.SetActive(true);

        player.GetComponent<PlayerController>().enabled = false;

        // 3. 페이드 아웃 (화면 밝아짐)
        fade.FadeOut();
        yield return new WaitUntil(() => !fade.isFadingOut);

        // 4. Dolly Cart 활성화 및 이동
        yield return StartCoroutine(OpenDoor());
        dollyCart.gameObject.SetActive(true); // Dolly Cart 활성화
        yield return new WaitForSeconds(cinemachineDuration); // 시네머신 카메라 연출 지속 시간 대기

        fade.FadeIn();
        // 5. 패널 전환 및 씬 전환 처리
        yield return StartCoroutine(HandlePanelTransition());
    }

    private IEnumerator HandlePanelTransition()
    {
        // 첫 번째 패널 처리
        yield return StartCoroutine(FadePanel(panel1CanvasGroup));

        // 두 번째 패널 처리
        yield return StartCoroutine(FadePanel(panel2CanvasGroup));

        // 3초 대기 후 씬 전환
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("0.Start"); // 변경하려는 씬 이름으로 대체
    }

    private IEnumerator FadePanel(CanvasGroup canvasGroup)
    {
        // 패널 페이드 인 (투명도 증가)
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 1f, panelFadeDuration));

        // 패널 1초 동안 유지 (필요에 따라 조정 가능)
        yield return new WaitForSeconds(1f);

        // 패널 페이드 아웃 (투명도 감소)
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


