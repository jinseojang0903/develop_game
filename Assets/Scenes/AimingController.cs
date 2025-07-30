using UnityEngine;
using System.Collections; // �ڷ�ƾ ����� ���� �߰�

public class AimingController : MonoBehaviour
{
    [Header("Ŀ�� ����")]
    public Texture2D defaultCursor;   // �⺻ Ŀ�� �̹���
    public Texture2D crosshairCursor; // ���ؼ� Ŀ�� �̹���

    [Header("���ؼ� ���� ����")]
    public LineRenderer aimingLine;   // Line Renderer ������Ʈ
    public Transform gunMuzzle;       // �ѱ� ��ġ (�Ѿ��� ���۵� ��ġ)

    [Header("�ѱ� ����")]
    public GameObject bulletPrefab;     // �Ѿ� ������
    public Light muzzleFlashLight;      // �ѱ� ���� ����Ʈ

    [Header("���� ����")]
    public AudioClip gunshotSound;      // �߻� ���� ����

    private AudioSource audioSource;    // ���带 ����� ������Ʈ

    private bool isAiming = false; // ���� ���� ������� Ȯ���ϴ� ����
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        // ���� ���� �� �⺻ Ŀ���� ����
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        if (muzzleFlashLight != null)
            muzzleFlashLight.enabled = false;
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        HandleAimingToggle();
        UpdateCursorAndLine();

        if (isAiming && Input.GetMouseButtonDown(0))
        {
            Shoot();
            audioSource.PlayOneShot(gunshotSound);
        }
    }

    void Shoot()
    {
        // 1. ���콺 �������� �Ѿ��� ���ư� ����� ȸ���� ���
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, gunMuzzle.position);
        float rayDistance;

        if (groundPlane.Raycast(cameraRay, out rayDistance))
        {
            Vector3 targetPoint = cameraRay.GetPoint(rayDistance);


            Vector3 direction = targetPoint - gunMuzzle.position;
            direction.y = 0; // Y���� �������� ����
            Quaternion bulletRotation = Quaternion.LookRotation(direction);

            // 2. �Ѿ� ����
            Instantiate(bulletPrefab, gunMuzzle.position, bulletRotation);

            // 3. �ѱ� ���� ȿ�� ����
            StartCoroutine(MuzzleFlash());
        }
    }

    // �ѱ� ������ ��� �״ٰ� ���� �ڷ�ƾ
    IEnumerator MuzzleFlash()
    {
        muzzleFlashLight.enabled = true; // ����Ʈ �ѱ�
        yield return new WaitForSeconds(0.05f); // ���� ª�� �ð� ���� ���
        muzzleFlashLight.enabled = false; // ����Ʈ ����
    }

    // ��Ŭ������ ���� ��带 �Ѱ� ���� �Լ�
    private void HandleAimingToggle()
    {
        // ���콺 ��Ŭ���� �� �� ������ ��
        if (Input.GetMouseButtonDown(1))
        {
            // isAiming ���¸� ������Ŵ (true -> false, false -> true)
            isAiming = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            // isAiming ���¸� ������Ŵ (true -> false, false -> true)
            isAiming = false;
        }
    }

    // ���� ���¿� ���� Ŀ���� ���ؼ��� ������Ʈ�ϴ� �Լ�
    private void UpdateCursorAndLine()
    {
        if (isAiming)
        {
            // ���� ����� ��
            // 1. Ŀ���� ���ؼ� ������� ���� (�ֽ����� �߾�����)
            Vector2 crosshairHotspot = new Vector2(crosshairCursor.width / 2, crosshairCursor.height / 2);
            Cursor.SetCursor(crosshairCursor, crosshairHotspot, CursorMode.Auto);

            // 2. ���ؼ�(Line Renderer) Ȱ��ȭ �� ��ġ ����
            aimingLine.enabled = true;

            // ���콺 ��ġ ��� (������ HandleAiming ���� Ȱ��)
            Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
            float rayDistance;

            if (groundPlane.Raycast(cameraRay, out rayDistance))
            {
                Vector3 targetPoint = cameraRay.GetPoint(rayDistance);

                // Line Renderer�� �������� ���� ����
                aimingLine.SetPosition(0, gunMuzzle.position); // ������: �ѱ�
                aimingLine.SetPosition(1, targetPoint);      // ����: ���콺 ���� ��ġ
            }
        }
        else
        {
            // ���� ��尡 �ƴ� ��
            // 1. Ŀ���� �⺻ ������� ����
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);

            // 2. ���ؼ� ��Ȱ��ȭ
            aimingLine.enabled = false;
        }
    }
}