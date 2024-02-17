using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public AudioClip footstepSound; 
    public float footstepInterval = 0.5f;

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
        if (rb.velocity.magnitude > 0.1f && Time.time > nextFootstepTime && MiddleDoorOpener.isDoorOpen)
        {
            PlayFootstepSound();
            nextFootstepTime = Time.time + footstepInterval;
        }
    }

    void PlayFootstepSound()
    {
        audioSource.PlayOneShot(footstepSound);
    }
}
