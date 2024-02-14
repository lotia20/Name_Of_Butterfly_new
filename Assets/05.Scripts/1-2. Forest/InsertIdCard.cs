using UnityEngine;

public class InsertIdCard : MonoBehaviour
{
    public CampingCarHighlighter campingCarHighlighter;
    public GameObject targetObject;

    private RotateIdInserter rotateIdInserter;

    private void Start()
    {
        // RotateIdInserter 오브젝트 찾기
        rotateIdInserter = FindObjectOfType<RotateIdInserter>();

        // RotateIdInserter의 RockRotated 이벤트에 함수 추가 (구독)
        if (rotateIdInserter != null)
        {
            rotateIdInserter.RockRotated.AddListener(OnRockRotated);
        }
    }

    // RockRotated 이벤트가 발생했을 때 호출되는 함수
    private void OnRockRotated()
    {
        // E 키를 눌렀을 때 campingCarHighlighter.UpdateOutline(targetObject) 실행
        if (Input.GetKeyDown(KeyCode.E))
        {
            campingCarHighlighter.UpdateOutline(targetObject);
        }
    }
}

