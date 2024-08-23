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

    FadeEffect fade;
    public GameObject door;
    public GameObject UpDoor;
    public GameObject DownDoor;

    private AudioSource audioSource;
    private bool soundPlayed = false;
    private bool isTriggered = false;
    private float cinemachineDuration = 5f; // 시네머신 연출 지속 시간

    void Start()
    {
        fade = FindObjectOfType<FadeEffect>();
        vcam.gameObject.SetActive(false); // 초기에는 시네머신 카메라 비활성화
        secondaryCamera.gameObject.SetActive(false); // 두 번째 카메라도 비활성화
        dollyCart.gameObject.SetActive(false); // Dolly Cart 비활성화
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

    }

    private IEnumerator MoveDollyCart()
    {
        float startTime = Time.time;
        float journeyLength = 5f; // 원하는 이동 시간

        while (Time.time - startTime < journeyLength)
        {
            float distanceCovered = (Time.time - startTime) / journeyLength;
            dollyCart.m_Position = Mathf.Lerp(0, 1, distanceCovered); // 0에서 1까지 이동
            yield return null;
        }

        dollyCart.m_Position = 1; // 최종 위치 설정
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



