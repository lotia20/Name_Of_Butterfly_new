using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 스피드 조정 변수
    [SerializeField]
    private float walkSpeed;
    [SerializeField] 
    private float runSpeed;
    [SerializeField] 
    private float crouchSpeed;
    private float applySpeed;

    // 점프 정도
    [SerializeField]
    private float jumpForce;

    // 상태 변수
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;
    public static bool isLoad{ get; set; } = false;

    // 앉은 상태일 때 얼마나 앉을 지 결정하는 변수
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

    // 줌 변수 
    private float xRotate = 0.0f;
    private float yRotate = 0.0f;
    public float zoomSpeed = 10.0f;

    // 뛸 때 카메라 흔들림 정도와 속도
    [SerializeField] 
    private float cameraBounceAmount = 0.01f; 
    [SerializeField]
    private float cameraBounceSpeed = 5f; 

    private bool isBouncing = false;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        rigidbody = GetComponent<Rigidbody>();

        // 초기화
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
        CameraControl();
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

    private void Jump()
    {
        if (isCrouch)
            Crouch();

        rigidbody.velocity = transform.up * jumpForce;
    }  

    private void TryRun()
    {
        if(!isLoad)
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
    }

    private void Running()
    {
        if (isCrouch)
            Crouch();

        isRun = true;
        applySpeed = runSpeed;

        if (!isBouncing)
        {
            StartCoroutine(CameraBounce());
        }
    }

    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;

        StopCoroutine(CameraBounce());
        isBouncing = false;
        camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, originPosY, camera.transform.localPosition.z);
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
        if (isCrouch)
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
        float posX = camera.transform.localPosition.x;
        float posY = camera.transform.localPosition.y;
        float posZ = camera.transform.localPosition.z;
        int count = 0;

        while(posY != applyCrouchPosY)
        {
            count++;
            posY = Mathf.Lerp(posY, applyCrouchPosY, 0.2f);
            camera.transform.localPosition = new Vector3(posX, posY, posZ);
            if(count > 15)
                break;
            yield return null;
        }
        camera.transform.localPosition = new Vector3(posX, applyCrouchPosY, posZ);
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

    private void CameraControl()
    {
        if(CleanController.isCleaning == true)
        {
            if(Input.GetMouseButton(0))
            {
                CameraRotation();
            }
        }
        else
        {
            CameraRotation();
        }
    }

    private void CameraRotation()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y"); 
        float cameraRotationX = xRotation * lookSensitivity;
        
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        camera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
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

    private IEnumerator CameraBounce()
    {
        isBouncing = true;
        float timer = 0f;

        while (isRun)
        {
            float newY = originPosY + Mathf.Sin(timer * cameraBounceSpeed) * cameraBounceAmount;
            camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, newY, camera.transform.localPosition.z);

            timer += Time.deltaTime;
            yield return null;
        }

        camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, originPosY, camera.transform.localPosition.z);
        isBouncing = false;
    }
}
