using UnityEngine;
using UnityEngine.Audio;

public class PuzzleManager : MonoBehaviour
{
    public GameObject growingCirclePrefab; // GrowingCircle 프리팹을 연결할 변수
    public Transform canvasTransform;     // 캔버스 Transform을 연결할 변수

    public AudioClip successSound;
    public AudioClip failureSound;
    private AudioSource audioSource;

    [Header("퍼즐 설정")]
    public RectTransform spawnArea;       // ★ 원이 생성될 영역 (배경 이미지)
    public float spawnInterval = 3f;      // ★ 생성 시간 간격 (3초)

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // PuzzleManager에 있는 AudioSource

        // 게임이 시작되면 원 생성 코루틴을 바로 실행합니다.
        StartCoroutine(CircleSpawningRoutine());

    }
    private System.Collections.IEnumerator CircleSpawningRoutine()
    {
        // 게임이 진행되는 동안 계속 반복합니다.
        while (true)
        {
            // 1. 정해진 시간만큼 대기합니다.
            yield return new WaitForSeconds(spawnInterval);

            // 2. spawnArea 내부의 랜덤 좌표를 계산합니다.
            Rect spawnRect = spawnArea.rect;
            float randomX = Random.Range(spawnRect.xMin, spawnRect.xMax);
            float randomY = Random.Range(spawnRect.yMin, spawnRect.yMax);
            Vector2 randomPosition = new Vector2(randomX, randomY);

            // 3. 프리팹을 spawnArea의 자식으로 생성합니다.
            GameObject newCircle = Instantiate(growingCirclePrefab, spawnArea);

            // 4. 생성된 원의 위치를 랜덤 좌표로 설정합니다.
            newCircle.GetComponent<RectTransform>().localPosition = randomPosition;
        }
    }

    void SpawnCircle()
    {
        // 캔버스 자식으로 프리팹을 생성
        GameObject newCircle = Instantiate(growingCirclePrefab, canvasTransform);
        // 여기서 newCircle의 위치를 랜덤하게 설정할 수 있음
    }

    // GrowingCircle에서 호출할 점수 획득 함수
    public void AddScore()
    {
        // 점수 올리는 로직
    }

    public void PlaySuccessSound()
    {
        audioSource.PlayOneShot(successSound);
    }

    public void PlayFailureSound()
    {
        audioSource.PlayOneShot(failureSound);
    }
}