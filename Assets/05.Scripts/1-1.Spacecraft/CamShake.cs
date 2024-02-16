using System.Collections;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    [SerializeField] private float force = 0f;  // 흔들림 세기  
    [SerializeField] private Vector3 offset = Vector3.zero; // 흔들림 축
    [SerializeField] private float shakeDuration = 5f; // 지속 시간
    public static bool isShaking{ get; private set; } = false;
    private Quaternion originalRot;

    private void Start()
    {
        originalRot = transform.rotation;
    }

    private void Update()
    {
        if (GunActiveController.isGunActivate)
        {
            StartCoroutine(ShakeAfterDelay(2f));
        }
    }  

    private IEnumerator ShakeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(Shake());
    }

    public IEnumerator Shake()
    {   
        isShaking = true;
        Vector3 originEuler = transform.eulerAngles;

        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
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
            Debug.Log(elapsedTime);
            yield return null;
        }
        Debug.Log("Shaking is done");
        StartCoroutine(Reset());
        yield return null;
    }

    public IEnumerator Reset()
    {
        while(Quaternion.Angle(transform.rotation, originalRot) > 0f)
        {
            isShaking = false;
            Debug.Log("Reset");
            transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRot, force * Time.deltaTime);
            yield return null;
        }
    }
}
