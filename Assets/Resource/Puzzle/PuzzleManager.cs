using UnityEngine;
using UnityEngine.Audio;

public class PuzzleManager : MonoBehaviour
{
    public GameObject growingCirclePrefab; // GrowingCircle �������� ������ ����
    public Transform canvasTransform;     // ĵ���� Transform�� ������ ����

    public AudioClip successSound;
    public AudioClip failureSound;
    private AudioSource audioSource;

    [Header("���� ����")]
    public RectTransform spawnArea;       // �� ���� ������ ���� (��� �̹���)
    public float spawnInterval = 3f;      // �� ���� �ð� ���� (3��)

    [Header("���� UI ����")]
    public GameObject puzzleCanvas; // �� ���� UI�� ��� ĵ���� ������Ʈ
    private Coroutine spawningCoroutine; // �� �������� �ڷ�ƾ�� ������ ����
    private bool isPuzzleActive = true; // �� ������ Ȱ��ȭ ����
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // PuzzleManager�� �ִ� AudioSource

        // ������ ���۵Ǹ� �� ���� �ڷ�ƾ�� �ٷ� �����մϴ�.

        StartPuzzle();
    }

    void Update()
    {
        // QŰ�� �� �� ������ ��
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isPuzzleActive = !isPuzzleActive; // Ȱ��ȭ ���¸� ����

            if (isPuzzleActive)
            {
                StartPuzzle(); // ���� ����
            }
            else
            {
                StopPuzzle(); // ���� ����
            }
        }
    }
    private System.Collections.IEnumerator CircleSpawningRoutine()
    {
        // ������ ����Ǵ� ���� ��� �ݺ��մϴ�.
        while (true)
        {
            // 1. ������ �ð���ŭ ����մϴ�.
            yield return new WaitForSeconds(spawnInterval);

            // 2. spawnArea ������ ���� ��ǥ�� ����մϴ�.
            Rect spawnRect = spawnArea.rect;
            float randomX = Random.Range(spawnRect.xMin, spawnRect.xMax);
            float randomY = Random.Range(spawnRect.yMin, spawnRect.yMax);
            Vector2 randomPosition = new Vector2(randomX, randomY);

            // 3. �������� spawnArea�� �ڽ����� �����մϴ�.
            GameObject newCircle = Instantiate(growingCirclePrefab, spawnArea);

            // 4. ������ ���� ��ġ�� ���� ��ǥ�� �����մϴ�.
            newCircle.GetComponent<RectTransform>().localPosition = randomPosition;
        }
    }

    void SpawnCircle()
    {
        // ĵ���� �ڽ����� �������� ����
        GameObject newCircle = Instantiate(growingCirclePrefab, canvasTransform);
        // ���⼭ newCircle�� ��ġ�� �����ϰ� ������ �� ����
    }

    // GrowingCircle���� ȣ���� ���� ȹ�� �Լ�
    public void AddScore()
    {
        // ���� �ø��� ����
    }

    public void PlaySuccessSound()
    {
        audioSource.PlayOneShot(successSound);
    }

    public void PlayFailureSound()
    {
        audioSource.PlayOneShot(failureSound);
    }
    // PuzzleManager.cs �� ���� �߰�
    void StartPuzzle()
    {
        puzzleCanvas.SetActive(true); // ĵ���� Ȱ��ȭ
                                      // �������� �ڷ�ƾ�� ���ٸ� ���� ����
        if (spawningCoroutine == null)
        {
            spawningCoroutine = StartCoroutine(CircleSpawningRoutine());
        }
    }

    void StopPuzzle()
    {
        puzzleCanvas.SetActive(false); // ĵ���� ��Ȱ��ȭ
                                       // �������� �ڷ�ƾ�� �ִٸ� ����
        if (spawningCoroutine != null)
        {
            StopCoroutine(spawningCoroutine);
            spawningCoroutine = null;
        }

        // ȭ�鿡 �����ִ� ��� ���� ã�� �ı� (�Ʒ� 2�� �׸� ����)
        GameObject[] remainingCircles = GameObject.FindGameObjectsWithTag("PuzzleCircle");
        foreach (GameObject circle in remainingCircles)
        {
            Destroy(circle);
        }
    }
}