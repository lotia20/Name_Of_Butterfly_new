using System.Collections;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    [SerializeField]
    private float roughness;      
    [SerializeField]
    private float magnitude;   

    void Update()
    {
        if(GunActiveController.isGunActivate)
        {
            StartCoroutine(Shake(1f));
        }
    }  

    public IEnumerator Shake(float duration)
    {
        float halfDuration = duration / 2;
        float elapsed = 0f;
        float tick = Random.Range(-10f, 10f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime / halfDuration;

            tick += Time.deltaTime * roughness;
            transform.position = new Vector3(
                Mathf.PerlinNoise(tick, 0) - .5f,
                Mathf.PerlinNoise(0, tick) - .5f,
                0f) * magnitude * Mathf.PingPong(elapsed, halfDuration);

            yield return null;
        }
    }
    // IEnumerator DelayedShake(float delay)
    // {
    //     yield return new WaitForSeconds(delay);

    //     // CamShake 스크립트의 Shake 메서드 호출
    //     StartCoroutine(Shake(1f));
    // }
}
