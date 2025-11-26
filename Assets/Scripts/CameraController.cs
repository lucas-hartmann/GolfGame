using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    public Transform ball;

    [Header("Orbit Settings")]
    public float rotateSpeed = 120f;
    public float pitchMin = 5f;
    public float pitchMax = 60f;

    [Header("Distance")]
    public float distance = 7f;
    public float minDistance = 4f;
    public float maxDistance = 12f;
    public float zoomSpeed = 5f;

    [Header("Smoothing")]
    public float followSmooth = 10f;

    private float yaw;   // left/right rotation
    private float pitch; // up/down angle

    void Start()
    {
        if (!ball) return;

        // Initialize yaw & pitch from current camera orientation
        Vector3 dir = (transform.position - ball.position).normalized;
        pitch = Mathf.Asin(dir.y) * Mathf.Rad2Deg;
        yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
    }

    void LateUpdate()
    {
        if (!ball) return;

        // --- ROTATE AROUND BALL WITH RIGHT MOUSE ---
        if (Input.GetMouseButton(1)) // hold right mouse button to rotate
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            yaw += mouseX * rotateSpeed * Time.deltaTime;
            pitch -= mouseY * rotateSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
        }

        // --- ZOOM WITH SCROLL WHEEL ---
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            distance = Mathf.Clamp(
                distance - scroll * zoomSpeed,
                minDistance,
                maxDistance
            );
        }

        // --- CALCULATE CAMERA POSITION AROUND BALL ---
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPos = ball.position - (rotation * Vector3.forward * distance);

        // Smooth follow of ball position
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, followSmooth * Time.deltaTime);

        transform.position = smoothedPos;
        transform.rotation = rotation;
    }
}
