using UnityEngine;
using UnityEngine.Audio;

public class PuzzleManager : MonoBehaviour
{
    public GameObject growingCirclePrefab; // GrowingCircle 프리팹을 연결할 변수
    public Transform canvasTransform;     // 캔버스 Transform을 연결할 변수

    public AudioClip successSound;
    public AudioClip failureSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // PuzzleManager에 있는 AudioSource
    }


    void Update()
    {
        // 테스트를 위해 스페이스 바를 누르면 새 원이 생성되게 함
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCircle();
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