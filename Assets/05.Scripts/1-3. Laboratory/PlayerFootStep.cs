using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public AudioClip footstepSound;
    public float footstepInterval = 1.92f;

    public Collider hallwayCollider; 

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

    private bool IsInHallway()
    {
        return hallwayCollider.bounds.Contains(transform.position);
    }

    void PlayFootstepSound()
    {
        audioSource.PlayOneShot(footstepSound);
    }
}

