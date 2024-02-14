using UnityEngine;

public class CampingCarHighlighter : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float distanceThreshold = 30f;
    [SerializeField] private float startWidth = 1f;
    [SerializeField] private float maxOutlineWidth = 7f;
    [SerializeField] private float pulseSpeed = 7f;

    private Outline outline;
    private bool increasingWidth = true;

    private void OnEnable()
    {
        outline = GetComponent<Outline>();
        outline.OutlineWidth = startWidth;
        outline.enabled = false;
    }

    public void UpdateOutline(GameObject targetObject)
    {
        if (targetObject != null && player != null)
        {
            float distanceToPlayer = Vector3.Distance(targetObject.transform.position, player.position);

            if (CanInteract(targetObject))
            {
                outline.enabled = true;
                AlterOutlineWidth();
            }
            else
            {
                outline.enabled = false;
            }
        }
    }

    private bool CanInteract(GameObject obj)
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(obj.transform.position);
        bool inCameraView = viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0;
        float distanceToPlayer = Vector3.Distance(obj.transform.position, player.transform.position);
        return inCameraView && distanceToPlayer <= distanceThreshold;
    }

    private void AlterOutlineWidth()
    {
        float currentWidth = outline.OutlineWidth;

        if (increasingWidth)
        {
            if (currentWidth >= maxOutlineWidth)
            {
                increasingWidth = false;
            }
        }
        else
        {
            if (currentWidth <= startWidth)
            {
                increasingWidth = true;
            }
        }

        if (increasingWidth)
        {
            outline.OutlineWidth += Time.deltaTime * pulseSpeed;
        }
        else
        {
            outline.OutlineWidth -= Time.deltaTime * pulseSpeed;
        }
    }
}



