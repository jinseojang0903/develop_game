using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;    
using UnityEngine.EventSystems; // �̺�Ʈ �ý��� ����� ���� �߰�
using UnityEngine.UI; // UI ��� ����� ���� �߰�

public class GrowingCircle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    public float growthDuration = 2f; // ���� ��ǥ ũ����� Ŀ���� �� �ɸ��� �ð�
    public Vector3 targetScale = Vector3.one; // ��ǥ ũ�� (TargetCircle�� ������ ũ��)

    private bool isMouseOver = false; // ���콺�� ���� �� ���� �ִ��� Ȯ��

    [Header("�̹��� ����")]
    public Sprite defaultSprite; // �⺻ �� �̹���
    public Sprite activeSprite;  // ��ǥ ũ�� ���� �� ����� �̹���

    private Image circleImage; // �ڽ��� Image ������Ʈ�� ���� ����

    // �� ��ũ��Ʈ�� ���۵� �� �ڵ����� ȣ��
    void Start()
    {
        circleImage = GetComponent<Image>();
        if (circleImage != null)
        {
            circleImage.sprite = defaultSprite; // ���� �̹����� �⺻���� ����
        }

        StartCoroutine(GrowthCoroutine());
    }

    // ���� ���� Ŀ���� ������ ���� �ڷ�ƾ
    private System.Collections.IEnumerator GrowthCoroutine()
    {
        // --- 1. ���� �ܰ� ---
        transform.localScale = Vector3.zero;
        float timer = 0f;
        while (timer < growthDuration)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, timer / growthDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;

        // --- 2. ���� ��� �ܰ� (Hit Window) ---
        circleImage.sprite = activeSprite; // �̹����� 'Ȱ��' ���·� ����

        float hitWindowTimer = 0.2f; // Ŭ���� ��ٷ� �� �ð� (0.2��)
        bool successfullyClicked = false;

        while (hitWindowTimer > 0)
        {
            // ���콺�� �� ���� �ְ�, ���� Ŭ���� �ߴٸ�
            if (isMouseOver && Input.GetMouseButtonDown(0))
            {
                successfullyClicked = true;
                break; // ���������� ��� ���� ��� Ż��
            }
            hitWindowTimer -= Time.deltaTime;
            yield return null;
        }

        // --- 3. ���� �ǰ� �ܰ� ---
        if (successfullyClicked)
        {
            Debug.Log("����! +1��");
            // PuzzleManager�� ã�� ���� ����� ���� ó���� ��û
            PuzzleManager puzzleManager = FindObjectOfType<PuzzleManager>();
            if (puzzleManager != null)
            {
                puzzleManager.PlaySuccessSound();
                // puzzleManager.AddScore(); // ���� �ø��� �Լ��� �̷� ������ ȣ��
            }
        }
        else
        {
            Debug.Log("����!");
            // PuzzleManager�� ã�� ���� ���� ����� ��û
            PuzzleManager puzzleManager = FindObjectOfType<PuzzleManager>();
            if (puzzleManager != null)
            {
                puzzleManager.PlayFailureSound(); // �� �� �κ��� �߰�
            }
        }

        // ������Ʈ �ı�
        Destroy(gameObject);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    // --- ���콺�� �� UI ��ҿ��� ������ �� �ڵ����� ȣ�� ---
    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }
}