using UnityEngine;
using UnityEngine.UI;

public interface IARCameraMovement
{
    void MoveForward();
    void MoveBackward();
    void MoveLeft();
    void MoveRight();
}

public class ARCameraMovement : MonoBehaviour, IARCameraMovement
{
    [Header("Movement Settings")]
    public float moveSpeed = 0.5f;

    [Header("Camera")]
    public Transform arCamera;

    [Header("UI Buttons")]
    public Button forwardButton;
    public Button backButton;
    public Button leftButton;
    public Button rightButton;

    void Start()
    {
        // Assign button listeners
        if (forwardButton != null) forwardButton.onClick.AddListener(MoveForward);
        Debug.Log("GoForward");
        if (backButton != null) backButton.onClick.AddListener(MoveBackward);
        Debug.Log("GoBackward");
        if (leftButton != null) leftButton.onClick.AddListener(MoveLeft);
        Debug.Log("Left");
        if (rightButton != null) rightButton.onClick.AddListener(MoveRight);
        Debug.Log("Right");
    }

    public void MoveForward()
    {
        transform.position += new Vector3(arCamera.forward.x, 0, arCamera.forward.z).normalized * moveSpeed;
    }

    public void MoveBackward()
    {
        transform.position -= new Vector3(arCamera.forward.x, 0, arCamera.forward.z).normalized * moveSpeed;
    }

    public void MoveLeft()
    {
        transform.position -= new Vector3(arCamera.right.x, 0, arCamera.right.z).normalized * moveSpeed;
    }

    public void MoveRight()
    {
        transform.position += new Vector3(arCamera.right.x, 0, arCamera.right.z).normalized * moveSpeed;
    }
}
