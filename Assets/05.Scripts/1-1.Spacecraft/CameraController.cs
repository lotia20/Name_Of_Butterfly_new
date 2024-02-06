using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float turnSpeed = 4.0f; // 마우스 회전 속도    
    private float xRotate = 0.0f; // 내부 사용할 X축 회전량은 별도 정의 ( 카메라 위 아래 방향 )
    private float yRotate = 0.0f;
    public float zoomSpeed = 10.0f;
    private Camera mainCamera;
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        
    }
    void Update()
    {
       Zoom();
       Rotate();
    }

    private void Zoom()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed;
        if(distance != 0)
        {
            float viewLimit = mainCamera.fieldOfView + distance;
            float minZoom = 30f;
            float maxZoom = 80f;

            mainCamera.fieldOfView = Mathf.Clamp(viewLimit, minZoom, maxZoom);


        }
    } 

    private void Rotate()
    {

        if(Input.GetMouseButton(1))
        {
             // 좌우로 움직인 마우스의 이동량 * 속도에 따라 카메라가 좌우로 회전할 양 계산
            float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
            // 현재 y축 회전값에 더한 새로운 회전각도 계산
            float yRotate = transform.eulerAngles.y + yRotateSize;

            // 위아래로 움직인 마우스의 이동량 * 속도에 따라 카메라가 회전할 양 계산(하늘, 바닥을 바라보는 동작)
            float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
            // 위아래 회전량을 더해주지만 -45도 ~ 80도로 제한 (-45:하늘방향, 80:바닥방향)
            // Clamp 는 값의 범위를 제한하는 함수
            xRotate = Mathf.Clamp(xRotate + xRotateSize, -60, 80);
    
            // 카메라 회전량을 카메라에 반영(X, Y축만 회전)
            transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
        }
    }
}
