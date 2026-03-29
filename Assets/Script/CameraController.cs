using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform player;

    public float mouseSensitivity = 200f;
    public float distance = 4f;
    public float height = 1.5f;
    public float smoothSpeed = 10f;

    [Header("Collision Settings")]
    public LayerMask collisionMask;
    public float minDistance = 1.0f;

    private float xRotation = 15f;
    private float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        Physics.queriesHitBackfaces = true;
        if (player == null) return;

        RotateCamera();
        FollowPlayer();
    }

    void RotateCamera()
    {
        Vector2 mouse = Mouse.current.delta.ReadValue();

        float mouseX = mouse.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouse.y * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -40f, 60f);

        // Rotate player with camera (Y axis only)
        player.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    void FollowPlayer()
    {
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);

        Vector3 pivot = player.position + Vector3.up * height;
        Vector3 desiredPosition = pivot + rotation * new Vector3(0, 0, -distance);

        Vector3 direction = desiredPosition - pivot;
        float distanceToCamera = direction.magnitude;

        RaycastHit hit;

        float minDistance = 0.3f;  // 🔥 important

        if (Physics.Raycast(pivot, direction.normalized, out hit, distanceToCamera))
        {
            distanceToCamera = hit.distance;
        }

        // ✅ THIS IS WHERE YOU USE IT
        float finalDistance = Mathf.Clamp(distanceToCamera, minDistance, distance);

        Vector3 finalPosition = pivot + direction.normalized * finalDistance;

        transform.position = Vector3.Lerp(
            transform.position,
            finalPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.LookAt(pivot);
    }
}