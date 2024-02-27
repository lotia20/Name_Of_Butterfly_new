using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public AudioClip footstepSound;
    public float footstepInterval = 1.92f;

    public Collider hallwayCollider; // 복도를 나타내는 Collider

    private AudioSource audioSource;
    private Rigidbody rb;
    private float nextFootstepTime;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        nextFootstepTime = 0f;
    }

    void Update()
    {
        // 플레이어의 속도가 0.1 이상이고, 복도 안에 있고, 일정 시간이 지났을 때 발소리 재생
        if (rb.velocity.magnitude > 0.1f && Time.time > nextFootstepTime && IsInHallway() && IsKeyPressed())
        {
            PlayFootstepSound();
            nextFootstepTime = Time.time + footstepInterval;
        }
    }

    private bool IsKeyPressed()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
    }

    // 플레이어가 복도 안에 있는지 여부를 확인하는 함수
    private bool IsInHallway()
    {
        return hallwayCollider.bounds.Contains(transform.position);
    }

    void PlayFootstepSound()
    {
        audioSource.PlayOneShot(footstepSound);
    }
}

