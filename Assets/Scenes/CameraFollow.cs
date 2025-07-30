using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 따라다닐 대상 (플레이어)
    public Vector3 offset;   // 카메라와 플레이어의 거리

    // LateUpdate는 모든 Update가 끝난 후 호출되어, 떨림 현상을 방지합니다.
    void LateUpdate()
    {
        if (target == null) return;

        // 카메라 위치 = 플레이어 위치 + 정해진 거리
        transform.position = target.position + offset;
    }
}