using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSmoothTime = 0.1f;
    public Camera cam;

    private CharacterController controller;
    private float turnSmoothVelocity;
    private Vector3 targetPosition;
    private bool isMovingToTouch = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleKeyboardMovement();
#else
        HandleTouchMovement();
#endif
    }

    // ------------------- PC movement (WASD) -------------------
    void HandleKeyboardMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(h, 0f, v).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
    }

    // ------------------- Mobile movement (Touch) -------------------
    void HandleTouchMovement()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Tap on ground
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = cam.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    targetPosition = hit.point;
                    isMovingToTouch = true;
                }
            }
        }

        if (isMovingToTouch)
        {
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0;

            if (direction.magnitude < 0.1f)
            {
                isMovingToTouch = false;
                return;
            }

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            controller.Move(direction.normalized * moveSpeed * Time.deltaTime);
        }
    }
}
