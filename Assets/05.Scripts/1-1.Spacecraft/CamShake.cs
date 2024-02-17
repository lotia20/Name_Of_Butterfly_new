using System.Collections;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    [SerializeField] private float force = 0f;  // 흔들림 세기  
    [SerializeField] private Vector3 offset = Vector3.zero; // 흔들림 축
    [SerializeField] private float shakeDuration = 5f; // 지속 시간
    public static bool isShaking{ get; private set; } = false;

    public IEnumerator ShakeAfterDelay(float delay)
    {
        Debug.Log("Shaking");
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(Shake());
    }

    public IEnumerator Shake()
    {   
        isShaking = true;
        Vector3 originEuler = transform.eulerAngles;

        float timer = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration && timer < 5f)
        {
            float rotX = Random.Range(-offset.x, offset.x);
            float rotY = Random.Range(-offset.y, offset.y);
            float rotZ = Random.Range(-offset.z, offset.z);

            Vector3 randomRot = originEuler + new Vector3(rotX, rotY, rotZ);
            Quaternion rot = Quaternion.Euler(randomRot);

            while(Quaternion.Angle(transform.rotation, rot) > 0.1f)
            {
                Debug.Log("Shaking");
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, force * Time.deltaTime);
                yield return null;
            }
            elapsedTime += Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        isShaking = false;
    }
}
