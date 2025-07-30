using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // ����ٴ� ��� (�÷��̾�)
    public Vector3 offset;   // ī�޶�� �÷��̾��� �Ÿ�

    // LateUpdate�� ��� Update�� ���� �� ȣ��Ǿ�, ���� ������ �����մϴ�.
    void LateUpdate()
    {
        if (target == null) return;

        // ī�޶� ��ġ = �÷��̾� ��ġ + ������ �Ÿ�
        transform.position = target.position + offset;
    }
}