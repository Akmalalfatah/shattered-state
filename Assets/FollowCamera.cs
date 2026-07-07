using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;

    public float followSpeed = 2f;

    public float followAmount = 0.15f;

    public float minX;
    public float maxX;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        float targetX =
            initialPosition.x +
            (target.position.x * followAmount);

        targetX = Mathf.Clamp(targetX, minX, maxX);

        Vector3 targetPosition = new Vector3(
            targetX,
            initialPosition.y,
            initialPosition.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );
    }
}