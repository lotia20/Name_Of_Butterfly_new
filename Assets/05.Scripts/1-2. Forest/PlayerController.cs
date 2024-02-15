using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //스피드 조정 변수
    [SerializeField]
    private float walkSpeed;
    [SerializeField] 
    private float runSpeed;
    [SerializeField] 
    private float crouchSpeed;
    private float applySpeed;

    //점프 정도
    [SerializeField]
     private float jumpForce;

     //상태변수
     private bool isRun = false;
     private bool isGround = true;
     private bool isCrouch = false;

     //앉은 상태일 때 얼마나 앉을 지 결정하는 변수
     [SerializeField] 
     private float crouchPosY;
     private float originPosY;
     private float applyCrouchPosY;

    [SerializeField]
    private float lookSensitivity; 

    [SerializeField]
    private float cameraRotationLimit;  
    private float currentCameraRotationX;  

    [SerializeField]
    private Camera camera; 
    private Rigidbody rigidbody;
    private CapsuleCollider capsuleCollider;

    //zoom 변수 
    private float xRotate = 0.0f; // 내부 사용할 X축 회전량은 별도 정의 ( 카메라 위 아래 방향 )
    private float yRotate = 0.0f;
    public float zoomSpeed = 10.0f;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        rigidbody = GetComponent<Rigidbody>();

        //초기화
        applySpeed = walkSpeed;

        originPosY = camera.transform.localPosition.y;
        Debug.Log(originPosY);
        applyCrouchPosY = originPosY;
    }

    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();
        PlayerRotation();

        Zoom();
    }

    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    // 점프
    private void Jump()
    {
        if (isCrouch)
            Crouch();

        rigidbody.velocity = transform.up * jumpForce;
    }  

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

     private void Running()
    {
        if (isCrouch)
            Crouch();

        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch;
        if (isCrouch) //앉은 상태
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    IEnumerator CrouchCoroutine()
    {
        float posY = camera.transform.localPosition.y;
        int count = 0;

        while(posY != applyCrouchPosY)
        {
            count++;
            posY = Mathf.Lerp(posY, applyCrouchPosY, 0.2f);
            camera.transform.localPosition = new Vector3(0, posY, 0);
            if(count > 15)
                break;
            yield return null;
        }
        camera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }


    private void Move()
    {
        float moveDirX = Input.GetAxis("Horizontal");  
        float moveDirY = Input.GetAxis("Vertical");
        Vector3 moveHorizontal = transform.right * moveDirX; 
        Vector3 moveVertical = transform.forward * moveDirY; 

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * applySpeed; 

        rigidbody.MovePosition(transform.position + velocity * Time.deltaTime);
    }
    
    private void CameraRotation()
    {
        if(CleanController.isCleaning == false)
        {
            float xRotation = Input.GetAxisRaw("Mouse Y"); 
            float cameraRotationX = xRotation * lookSensitivity;
        
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            camera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
    }

    private void PlayerRotation()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 playerRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(playerRotationY)); 
    }

    private void Zoom()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed;
        if(distance != 0)
        {
            float viewLimit = camera.fieldOfView + distance;
            float minZoom = 30f;
            float maxZoom = 80f;

            camera.fieldOfView = Mathf.Clamp(viewLimit, minZoom, maxZoom);


        }
    } 
}
