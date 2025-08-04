using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;      // Player hoặc object cần camera theo dõi
    public Vector3 offset;        // Khoảng cách giữa camera và target (thường là (0, 0, -10))
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
