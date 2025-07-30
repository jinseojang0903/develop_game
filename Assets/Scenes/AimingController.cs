using UnityEngine;
using System.Collections; // 코루틴 사용을 위해 추가

public class AimingController : MonoBehaviour
{
    [Header("커서 설정")]
    public Texture2D defaultCursor;   // 기본 커서 이미지
    public Texture2D crosshairCursor; // 조준선 커서 이미지

    [Header("조준선 라인 설정")]
    public LineRenderer aimingLine;   // Line Renderer 컴포넌트
    public Transform gunMuzzle;       // 총구 위치 (총알이 시작될 위치)

    [Header("총기 설정")]
    public GameObject bulletPrefab;     // 총알 프리팹
    public Light muzzleFlashLight;      // 총구 섬광 라이트

    [Header("사운드 설정")]
    public AudioClip gunshotSound;      // 발사 사운드 파일

    private AudioSource audioSource;    // 사운드를 재생할 컴포넌트

    private bool isAiming = false; // 현재 조준 모드인지 확인하는 변수
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        // 게임 시작 시 기본 커서로 설정
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
        // 1. 마우스 방향으로 총알이 날아갈 방향과 회전값 계산
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, gunMuzzle.position);
        float rayDistance;

        if (groundPlane.Raycast(cameraRay, out rayDistance))
        {
            Vector3 targetPoint = cameraRay.GetPoint(rayDistance);


            Vector3 direction = targetPoint - gunMuzzle.position;
            direction.y = 0; // Y축은 수평으로 고정
            Quaternion bulletRotation = Quaternion.LookRotation(direction);

            // 2. 총알 생성
            Instantiate(bulletPrefab, gunMuzzle.position, bulletRotation);

            // 3. 총구 섬광 효과 실행
            StartCoroutine(MuzzleFlash());
        }
    }

    // 총구 섬광을 잠깐 켰다가 끄는 코루틴
    IEnumerator MuzzleFlash()
    {
        muzzleFlashLight.enabled = true; // 라이트 켜기
        yield return new WaitForSeconds(0.05f); // 아주 짧은 시간 동안 대기
        muzzleFlashLight.enabled = false; // 라이트 끄기
    }

    // 우클릭으로 조준 모드를 켜고 끄는 함수
    private void HandleAimingToggle()
    {
        // 마우스 우클릭을 한 번 눌렀을 때
        if (Input.GetMouseButtonDown(1))
        {
            // isAiming 상태를 반전시킴 (true -> false, false -> true)
            isAiming = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            // isAiming 상태를 반전시킴 (true -> false, false -> true)
            isAiming = false;
        }
    }

    // 조준 상태에 따라 커서와 조준선을 업데이트하는 함수
    private void UpdateCursorAndLine()
    {
        if (isAiming)
        {
            // 조준 모드일 때
            // 1. 커서를 조준선 모양으로 변경 (핫스팟은 중앙으로)
            Vector2 crosshairHotspot = new Vector2(crosshairCursor.width / 2, crosshairCursor.height / 2);
            Cursor.SetCursor(crosshairCursor, crosshairHotspot, CursorMode.Auto);

            // 2. 조준선(Line Renderer) 활성화 및 위치 설정
            aimingLine.enabled = true;

            // 마우스 위치 계산 (기존의 HandleAiming 로직 활용)
            Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
            float rayDistance;

            if (groundPlane.Raycast(cameraRay, out rayDistance))
            {
                Vector3 targetPoint = cameraRay.GetPoint(rayDistance);

                // Line Renderer의 시작점과 끝점 설정
                aimingLine.SetPosition(0, gunMuzzle.position); // 시작점: 총구
                aimingLine.SetPosition(1, targetPoint);      // 끝점: 마우스 월드 위치
            }
        }
        else
        {
            // 조준 모드가 아닐 때
            // 1. 커서를 기본 모양으로 변경
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);

            // 2. 조준선 비활성화
            aimingLine.enabled = false;
        }
    }
}