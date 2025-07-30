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

        if (animator.GetBool("RightButton")) // ���콺 ��Ŭ���� ������ �ִ� ����
        {
            // ���� ȸ�� ����
            HandleAiming();
        }
        else // ���콺 ��Ŭ���� ������ �ʾ��� ��
        {
            // �̵� �������� ȸ�� ����
            HandleRotationByMovement();
        }
    }

    void HandleMovement()
    {
        // 1. Ű���� �Է� �ޱ� (A/D�� X��, W/S�� Y������ ���)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // 2. �̵� ���� ���� ���� (Z�� ���� 0����, Y�� ���� �Է��� ����)
        // �ڡڡ� ���Ⱑ �ٽ� �������Դϴ� �ڡڡ�
        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;
        
        // 3. ĳ���� �̵� ó��
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // 4. �ִϸ����Ϳ� �ӵ� ����
        animator.SetFloat("Speed", moveDirection.magnitude);
    }

    void HandleAiming()
    {
        // ���� ���� �ڵ�� ���� ���� �״�� �Ӵϴ�.
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
        // �̵� ���⿡ ���� ȸ�� ó��
        Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }


















}

