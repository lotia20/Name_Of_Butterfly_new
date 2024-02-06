using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    AudioSource highlightSource;
    public static bool IsOutlineEnabled { get; private set; } = false;
    public static GameObject ClosestObject { get; private set; }

    private Transform highlight;
    private Transform selection;
    private bool isSoundPlaying = false;


    void Start()
    {
        highlightSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Highlight
        if (highlight != null)
        {
            UpdateOutlineStatus(false);
            highlight = null;
        }

        // ���̶���Ʈ ���� �±� -> ����Ʈ�� �߰� �� �����Ͽ� ����
        string[] tagsToFind = { "SelectableDrawing", "SelectablePasswordScreen" };
        GameObject closestObject = FindClosestObject(tagsToFind);
        if (closestObject != null)
        {
            highlight = closestObject.transform;
            ClosestObject = closestObject;

            if (highlight.tag.Contains("Selectable") && highlight != selection)
            {
                //outline�̶�� ������Ʈ�� CO�� �߰���
                if (highlight.gameObject.TryGetComponent(out Outline outlineComponent))
                {
                    outlineComponent.enabled = true;
                    UpdateOutlineStatus(true);

                }
                //���� ������ �β� �� ���� �������� ������ ��ũ��Ʈ���� ��������
                else
                {
                    Outline outline = highlight.gameObject.AddComponent<Outline>();
                    outline.OutlineColor = Color.cyan;
                    outline.OutlineWidth = 5.0f;
                    outline.enabled = true;
                    UpdateOutlineStatus(true);
                }
            }
            else
            {
                highlight = null;
            }
        }

        // Selection : 
        if (closestObject != null)
        {
            if (selection != null)
            {
                Outline outlineComponent = selection.gameObject.GetComponent<Outline>();
                if (outlineComponent != null)
                {
                    outlineComponent.enabled = false;
                    UpdateOutlineStatus(false);
                }

                selection = closestObject.transform;
                outlineComponent = selection.gameObject.GetComponent<Outline>();
                if (outlineComponent != null && IsObjectInView(closestObject))
                {
                    outlineComponent.enabled = true;
                    UpdateOutlineStatus(true);
                }

                highlight = null;
            }
            else
            {
                selection = closestObject.transform;
                Outline outlineComponent = selection.gameObject.GetComponent<Outline>();

                if (outlineComponent != null)
                {
                    outlineComponent.enabled = true;
                    UpdateOutlineStatus(true);
                }

            }
        }

        if (IsOutlineEnabled && IsObjectInView(ClosestObject) && !isSoundPlaying)
        {
            PlaySelectionSound();
        }

        if (!IsObjectInView(ClosestObject))
        {
            isSoundPlaying = false;
        }
    }
    bool IsObjectInView(GameObject obj)
    {
        //�÷��̾� ������Ʈ �����Ÿ� 2f�� ���� -> ���氡��
        float maxDistance = 2f;
        if (obj != null)
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(obj.transform.position);
            float distance = Vector3.Distance(Camera.main.transform.position, obj.transform.position);
            return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1 && distance <= maxDistance;
        }
        return false;
    }

    void UpdateOutlineStatus(bool newStatus)
    {
        IsOutlineEnabled = newStatus;
    }

    GameObject FindClosestObject(string[] tags)
    {
        GameObject[] allObjects = FindObjectsWithTags(tags);
        GameObject closestObject = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject obj in allObjects)
        {
            float distance = Vector3.Distance(Camera.main.transform.position, obj.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj;
            }
        }

        return closestObject;
    }

    GameObject[] FindObjectsWithTags(string[] tags)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        System.Collections.Generic.List<GameObject> selectedObjects = new System.Collections.Generic.List<GameObject>();
        foreach (string tag in tags)
        {
            foreach (GameObject obj in allObjects)
            {
                if (obj.CompareTag(tag))
                {
                    selectedObjects.Add(obj);
                }
            }
        }
        return selectedObjects.ToArray();
    }

    void PlaySelectionSound()
    {
        if (highlightSource != null && highlightSource.clip != null)
        {
            highlightSource.PlayOneShot(highlightSource.clip);
            isSoundPlaying = true;
        }
    }
}
