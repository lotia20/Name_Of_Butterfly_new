using UnityEngine;

public class InsertIdCard : MonoBehaviour
{
    public CampingCarHighlighter campingCarHighlighter;
    public GameObject targetObject;

    private RotateIdInserter rotateIdInserter;

    private void Start()
    {
        // RotateIdInserter ������Ʈ ã��
        rotateIdInserter = FindObjectOfType<RotateIdInserter>();

        // RotateIdInserter�� RockRotated �̺�Ʈ�� �Լ� �߰� (����)
        if (rotateIdInserter != null)
        {
            rotateIdInserter.RockRotated.AddListener(OnRockRotated);
        }
    }

    // RockRotated �̺�Ʈ�� �߻����� �� ȣ��Ǵ� �Լ�
    private void OnRockRotated()
    {
        // E Ű�� ������ �� campingCarHighlighter.UpdateOutline(targetObject) ����
        if (Input.GetKeyDown(KeyCode.E))
        {
            campingCarHighlighter.UpdateOutline(targetObject);
        }
    }
}

