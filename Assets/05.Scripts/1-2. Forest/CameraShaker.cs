using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public float roughness = 1f; 
    public float magnitude = 1f;
    public GameObject player;

    public IEnumerator Shake(float duration)
    {
        float halfDuration = duration / 2;
        float elapsed = 0f;
        float tick = Random.Range(-1f, 1f);

        Vector3 originalPosition = transform.localPosition; 

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime / halfDuration;

            tick += Time.deltaTime * roughness;
            Vector3 shakeOffset = new Vector3(
                Mathf.PerlinNoise(tick, 0) - 0.5f,
                Mathf.PerlinNoise(0, tick) - 0.5f,
                0f) * magnitude * Mathf.PingPong(elapsed, halfDuration);

            transform.localPosition = originalPosition + shakeOffset;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}

