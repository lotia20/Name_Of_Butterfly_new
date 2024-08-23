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

    FadeEffect fade;
    public GameObject door;
    public GameObject UpDoor;
    public GameObject DownDoor;

    private AudioSource audioSource;
    private bool soundPlayed = false;
    private bool isTriggered = false;
    private float cinemachineDuration = 5f; // �ó׸ӽ� ���� ���� �ð�

    void Start()
    {
        fade = FindObjectOfType<FadeEffect>();
        vcam.gameObject.SetActive(false); // �ʱ⿡�� �ó׸ӽ� ī�޶� ��Ȱ��ȭ
        secondaryCamera.gameObject.SetActive(false); // �� ��° ī�޶� ��Ȱ��ȭ
        dollyCart.gameObject.SetActive(false); // Dolly Cart ��Ȱ��ȭ
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

    }

    private IEnumerator MoveDollyCart()
    {
        float startTime = Time.time;
        float journeyLength = 5f; // ���ϴ� �̵� �ð�

        while (Time.time - startTime < journeyLength)
        {
            float distanceCovered = (Time.time - startTime) / journeyLength;
            dollyCart.m_Position = Mathf.Lerp(0, 1, distanceCovered); // 0���� 1���� �̵�
            yield return null;
        }

        dollyCart.m_Position = 1; // ���� ��ġ ����
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



