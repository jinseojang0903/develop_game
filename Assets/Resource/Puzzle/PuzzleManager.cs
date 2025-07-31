using UnityEngine;
using UnityEngine.Audio;

public class PuzzleManager : MonoBehaviour
{
    public GameObject growingCirclePrefab; // GrowingCircle �������� ������ ����
    public Transform canvasTransform;     // ĵ���� Transform�� ������ ����

    public AudioClip successSound;
    public AudioClip failureSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // PuzzleManager�� �ִ� AudioSource
    }


    void Update()
    {
        // �׽�Ʈ�� ���� �����̽� �ٸ� ������ �� ���� �����ǰ� ��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCircle();
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
}