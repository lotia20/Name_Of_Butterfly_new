using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorOpener : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float openSpeed;

    public GameObject door;
    public GameObject doorLight;
    public GameObject UpDoor;
    public GameObject DownDoor;
    public float interactionDistance = 2f;

    private bool isDoorOpen = false;

    void Update()
    {
        if (RotateIdInserter.IsDoorOpened && IsPlayerNearDoor())
        {
            OpenDoor();
            TurnOnDoorLight();
            isDoorOpen = true;
        }
    }

    bool IsPlayerNearDoor()
    {
        float distanceToPlayer = Vector3.Distance(door.transform.position, player.position);
        return distanceToPlayer <= interactionDistance;
    }

    void OpenDoor()
    {
        if (UpDoor.transform.position.y <= 30f)
            UpDoor.transform.Translate(Vector3.up * Time.deltaTime * openSpeed);
        if (DownDoor.transform.position.y >= -6f)
            DownDoor.transform.Translate(Vector3.down * Time.deltaTime * openSpeed);
    }
    void TurnOnDoorLight()
    {
        Renderer doorLightRenderer = doorLight.GetComponent<Renderer>();
        if (doorLightRenderer != null)
        {
            doorLightRenderer.material.SetColor("_EmissionColor", Color.cyan);

            float intensity = 25f;
            Color emissionColor = Color.cyan * intensity;
            doorLightRenderer.material.SetColor("_EmissionColor", emissionColor);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isDoorOpen)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
