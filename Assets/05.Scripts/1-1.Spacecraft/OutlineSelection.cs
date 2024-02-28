using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OutlineSelection : MonoBehaviour
{
    [SerializeField] private AudioSource highlightSource;
    [SerializeField] private string[] selectableTags = { "SelectableDrawing", "SelectablePasswordScreen", "SelectableIdCard" };
    [SerializeField] private float maxDistance;
    public TutorialExpose tutorialExpose;
    public GameObject EKeyUi;
    public static bool IsOutlineEnabled { get; private set; } = false;
    public static GameObject ClosestObject { get; private set; }

    private Transform selection;
    private static Dictionary<GameObject, bool> objectSoundPlayedMap = new Dictionary<GameObject, bool>();

    private bool tutorialExposed = false;

    void Start()
    {
        highlightSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GameObject closestObject = FindClosestObject(selectableTags);
        
        if (closestObject != null && IsObjectInView(closestObject))
        {
            UpdateClosestObject(closestObject);

            if (ShouldHighlight(closestObject))
            {
                ProcessHighlight(closestObject);

                if (!tutorialExposed && BasicTutorial.IsEkeyEnabled) 
                {
                    tutorialExpose.SetImage(EKeyUi);
                    tutorialExpose.ShowAndHideImage(KeyCode.E);
                    tutorialExposed = true;
                }
            }
           
            ProcessSelection();
        }
        else
        {
            ClearHighlightAndSelection();
        }
    }

    bool ShouldHighlight(GameObject obj)
    {
        return obj.tag.Contains("Selectable") && obj.transform != selection && IsObjectInView(obj);
    }

    void ProcessHighlight(GameObject obj)
    {
        ClearOutlineComponent(selection);
        AddOrUpdateOutlineComponent(obj);
        selection = obj.transform;
        PlaySelectionSound(obj);
    }


    void ProcessSelection()
    {
        if (selection != null)
        {
            AddOrUpdateOutlineComponent(selection.gameObject);
        }
    }

    public void ClearHighlightAndSelection()
    {
        ClearOutlineComponent(selection);
        selection = null;
    }

    void AddOrUpdateOutlineComponent(GameObject obj)
    {
        if (obj.TryGetComponent(out Outline outlineComponent))
        {
            outlineComponent.enabled = true;
            outlineComponent.OutlineMode = Outline.Mode.OutlineVisible;
            UpdateOutlineStatus(true);
        }
        else
        {
            Outline outline = obj.AddComponent<Outline>();
            outline.OutlineColor = Color.cyan;
            outline.OutlineWidth = 4.0f;
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.enabled = true;
            UpdateOutlineStatus(true);
        }
    }

    void ClearOutlineComponent(Transform objTransform)
    {
        if (objTransform != null)
        {
            Outline outlineComponent = objTransform.gameObject.GetComponent<Outline>();
            if (outlineComponent != null)
            {
                outlineComponent.enabled = false;
                UpdateOutlineStatus(false);
            }
        }
    }

    void UpdateClosestObject(GameObject obj)
    {
        ClosestObject = obj;
    }


    bool IsObjectInView(GameObject obj)
    {
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

    void PlaySelectionSound(GameObject obj)
    {
        if (!objectSoundPlayedMap.ContainsKey(obj) || !objectSoundPlayedMap[obj])
        {
            if (highlightSource != null && highlightSource.clip != null)
            {
                highlightSource.PlayOneShot(highlightSource.clip);
                objectSoundPlayedMap[obj] = true;
            }
        }
    }
}