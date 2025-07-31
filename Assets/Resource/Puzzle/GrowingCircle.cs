using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;    
using UnityEngine.EventSystems; // 이벤트 시스템 사용을 위해 추가
using UnityEngine.UI; // UI 요소 사용을 위해 추가

public class GrowingCircle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    public float growthDuration = 2f; // 원이 목표 크기까지 커지는 데 걸리는 시간
    public Vector3 targetScale = Vector3.one; // 목표 크기 (TargetCircle과 동일한 크기)

    private bool isMouseOver = false; // 마우스가 현재 원 위에 있는지 확인

    [Header("이미지 설정")]
    public Sprite defaultSprite; // 기본 원 이미지
    public Sprite activeSprite;  // 목표 크기 도달 시 변경될 이미지

    private Image circleImage; // 자신의 Image 컴포넌트를 담을 변수

    // 이 스크립트가 시작될 때 자동으로 호출
    void Start()
    {
        circleImage = GetComponent<Image>();
        if (circleImage != null)
        {
            circleImage.sprite = defaultSprite; // 시작 이미지를 기본으로 설정
        }

        StartCoroutine(GrowthCoroutine());
    }

    // 원이 점점 커지는 로직을 담은 코루틴
    private System.Collections.IEnumerator GrowthCoroutine()
    {
        // --- 1. 성장 단계 ---
        transform.localScale = Vector3.zero;
        float timer = 0f;
        while (timer < growthDuration)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, timer / growthDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;

        // --- 2. 판정 대기 단계 (Hit Window) ---
        circleImage.sprite = activeSprite; // 이미지를 '활성' 상태로 변경

        float hitWindowTimer = 0.2f; // 클릭을 기다려 줄 시간 (0.2초)
        bool successfullyClicked = false;

        while (hitWindowTimer > 0)
        {
            // 마우스가 원 위에 있고, 왼쪽 클릭을 했다면
            if (isMouseOver && Input.GetMouseButtonDown(0))
            {
                successfullyClicked = true;
                break; // 성공했으니 대기 루프 즉시 탈출
            }
            hitWindowTimer -= Time.deltaTime;
            yield return null;
        }

        // --- 3. 최종 판결 단계 ---
        if (successfullyClicked)
        {
            Debug.Log("성공! +1점");
            // PuzzleManager를 찾아 사운드 재생과 점수 처리를 요청
            PuzzleManager puzzleManager = FindObjectOfType<PuzzleManager>();
            if (puzzleManager != null)
            {
                puzzleManager.PlaySuccessSound();
                // puzzleManager.AddScore(); // 점수 올리는 함수도 이런 식으로 호출
            }
        }
        else
        {
            Debug.Log("실패!");
            // PuzzleManager를 찾아 실패 사운드 재생을 요청
            PuzzleManager puzzleManager = FindObjectOfType<PuzzleManager>();
            if (puzzleManager != null)
            {
                puzzleManager.PlayFailureSound(); // ★ 이 부분을 추가
            }
        }

        // 오브젝트 파괴
        Destroy(gameObject);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    // --- 마우스가 이 UI 요소에서 나갔을 때 자동으로 호출 ---
    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }
}