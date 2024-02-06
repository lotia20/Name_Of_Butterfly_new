using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;
    private float applySpeed;

    //점프 정도
    [SerializeField] private float jumpForce;

    //상태 변수
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    //앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField] private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    //민감도
    [SerializeField] private float lookSensitivity;

    //필요한컴포넌트
    [SerializeField] private Camera camera;
    public CharacterController characterController;
    private CapsuleCollider capsuleCollider;
    float yVelocity = 0;

    // Start is called before the first frame update
    void Start()
    {
        //컴포넌트 할당
        characterController = GetComponent<CharacterController>();
        capsuleCollider = GetComponent<CapsuleCollider>();

         //초기화
        applySpeed = walkSpeed;

        originPosY = camera.transform.localPosition.y;
        applyCrouchPosY = originPosY;

    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();

        RotateWithCamera();
    }

     private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    //점프 시도
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    //점프
    private void Jump()
    {
        if (isCrouch)
            Crouch();

        characterController.Move(Vector3.up * jumpForce * Time.deltaTime);
    }

    //달리기 시도
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

    // 달리기
    private void Running()
    {
        if (isCrouch)
            Crouch();

        isRun = true;
        applySpeed = runSpeed;
    }

    // 달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

     // 앉기 시도
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    // 앉기 동작
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

     // 부드러운 앉기 동작
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
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 카메라의 forward 방향과 right 방향으로만 이동하도록 설정
        Vector3 forwardDirection = camera.transform.forward;
        forwardDirection.y = 0f;  // Y 축 방향은 0으로 설정하여 수직 이동을 방지

        Vector3 rightDirection = camera.transform.right;
        rightDirection.y = 0f;  // Y 축 방향은 0으로 설정하여 수직 이동을 방지

        Vector3 moveDirection = (forwardDirection.normalized * vertical) + (rightDirection.normalized * horizontal);
        moveDirection.Normalize();  // 방향 벡터를 정규화하여 이동 거리를 균일하게 조절

        moveDirection *= applySpeed;

        characterController.Move(moveDirection * Time.deltaTime);
        // float horizontal = Input.GetAxis("Horizontal");
        // float vertical = Input.GetAxis("Vertical");

        // Vector3 moveDirection = new Vector3(horizontal, 0, vertical);

        // moveDirection = camera.transform.TransformDirection(moveDirection);

        // moveDirection *= applySpeed;

        // characterController.Move(moveDirection * Time.deltaTime);
       
    }
    private void RotateWithCamera()
    {
        if(Input.GetMouseButton(1))
        {
            float horizontal = Input.GetAxis("Mouse X") * camera.GetComponent<CameraController>().turnSpeed;
            // 플레이어의 현재 회전값을 가져옴
            Vector3 currentRotation = transform.rotation.eulerAngles;

            // 좌우 회전값만 변경하여 새로운 회전값을 계산
            Vector3 newRotation = new Vector3(currentRotation.x, currentRotation.y + horizontal, currentRotation.z);

            // 플레이어의 회전을 적용
            transform.rotation = Quaternion.Euler(newRotation);
        }
    }
}
