using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 50f;    // �Ѿ� �ӵ�
    public float lifeTime = 2f;  // �Ѿ� ���� �ð�

    void Start()
    {
        // Rigidbody�� �����ͼ� ������(Z��) �ӵ��� ����
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;

        // lifeTime �� �Ŀ� �ڵ����� �� ���� ������Ʈ�� �ı�
        Destroy(gameObject, lifeTime);
    }
}