using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Aiming")]
    [SerializeField] private float rotationSpeed = 20f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
        animator.SetBool("RightButton", Input.GetMouseButton(1));

        if (animator.GetBool("RightButton")) // 마우스 우클릭을 누르고 있는 동안
        {
            // 조준 회전 실행
            HandleAiming();
        }
        else // 마우스 우클릭을 누르지 않았을 때
        {
            // 이동 방향으로 회전 실행
            HandleRotationByMovement();
        }
    }

    void HandleMovement()
    {
        // 1. 키보드 입력 받기 (A/D는 X축, W/S는 Y축으로 사용)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // 2. 이동 방향 벡터 생성 (Z축 값을 0으로, Y축 값에 입력을 넣음)
        // ★★★ 여기가 핵심 변경점입니다 ★★★
        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;
        
        // 3. 캐릭터 이동 처리
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // 4. 애니메이터에 속도 전달
        animator.SetFloat("Speed", moveDirection.magnitude);
    }

    void HandleAiming()
    {
        // 조준 관련 코드는 변경 없이 그대로 둡니다.
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
        float rayDistance;

        if (groundPlane.Raycast(cameraRay, out rayDistance))
        {
            Vector3 targetPoint = cameraRay.GetPoint(rayDistance);
            Vector3 lookDirection = targetPoint - transform.position;
            lookDirection.y = 0f;

            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }


    void HandleRotationByMovement()
    {
        // 이동 방향에 따라 회전 처리
        Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }


















}

