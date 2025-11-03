using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 2f;
    public float smoothSpeed = 10f;
    public float height = 1.7f; // camera height from the ground

    private float yaw;
    private float pitch;

    void LateUpdate()
    {
        if (!target) return;

#if UNITY_EDITOR || UNITY_STANDALONE
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
#else
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                yaw += touch.deltaPosition.x * 0.2f;
                pitch -= touch.deltaPosition.y * 0.2f;
            }
        }
#endif

        // Limit vertical look
        pitch = Mathf.Clamp(pitch, -60f, 85f);

        // Create rotation for camera
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Camera position is right at the player's head level
        Vector3 desiredPosition = target.position + Vector3.up * height;

        // Smoothly move camera
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Apply rotation to camera
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, smoothSpeed * Time.deltaTime);
    }


}
