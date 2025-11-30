using UnityEngine;

public class CameraRigController : MonoBehaviour
{
    [Header("Target")]
    public Transform ball;

    public enum CameraMode { FollowPOV, Helicopter }
    [Header("Mode")]
    public CameraMode currentMode = CameraMode.FollowPOV;

    [Header("Close View")]
    public float followPitchMin = 5f;
    public float followPitchMax = 60f;
    public float followDistance = 7f;
    public float followMinDistance = 4f;
    public float followMaxDistance = 12f;

    [Header("Helicopter View")]
    public float helicopterPitch = 60f;
    public float helicopterDistance = 20f;
    public float helicopterMinDistance = 10f;
    public float helicopterMaxDistance = 40f;

    [Header("Shared Settings")]
    public float rotateSpeed = 120f;
    public float zoomSpeed = 5f;
    public float followSmooth = 10f;
    public float helicopterSmooth = 5f;

    private float yaw;   // left/right rotation around ball
    private float pitch; // up/down angle

    void Start()
    {
        if (!ball) return;

        // Initialize yaw & pitch from current rig orientation
        Vector3 dir = (transform.position - ball.position).normalized;
        pitch = Mathf.Asin(dir.y) * Mathf.Rad2Deg;
        yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        // Ensure starting pitch is in a sane range
        pitch = Mathf.Clamp(pitch, followPitchMin, followPitchMax);
    }

    void LateUpdate()
    {
        if (!ball) return;

        // ----- SWITCH MODE (C) -----
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (currentMode == CameraMode.FollowPOV)
            {
                currentMode = CameraMode.Helicopter;
                pitch = helicopterPitch;          // fixed angle from above
            }
            else
            {
                currentMode = CameraMode.FollowPOV;
                pitch = Mathf.Clamp(pitch, followPitchMin, followPitchMax);
            }
        }

        // ----- ROTATION WITH RMB -----
        if (Input.GetMouseButton(1))   // hold right mouse button
        {
            float mouseX = Input.GetAxis("Mouse X");
            yaw += mouseX * rotateSpeed * Time.deltaTime;

            if (currentMode == CameraMode.FollowPOV)
            {
                float mouseY = Input.GetAxis("Mouse Y");
                pitch -= mouseY * rotateSpeed * Time.deltaTime;
                pitch = Mathf.Clamp(pitch, followPitchMin, followPitchMax);
            }
            else
            {
                // helicopter mode keeps a fixed pitch
                pitch = helicopterPitch;
            }
        }

        // ----- ZOOM WITH SCROLL WHEEL -----
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            if (currentMode == CameraMode.FollowPOV)
            {
                followDistance = Mathf.Clamp(
                    followDistance - scroll * zoomSpeed,
                    followMinDistance,
                    followMaxDistance
                );
            }
            else
            {
                helicopterDistance = Mathf.Clamp(
                    helicopterDistance - scroll * zoomSpeed,
                    helicopterMinDistance,
                    helicopterMaxDistance
                );
            }
        }

        // ----- CALCULATE CAMERA POSITION -----
        float usedDistance = (currentMode == CameraMode.FollowPOV)
            ? followDistance
            : helicopterDistance;

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // position rig on a sphere around the ball (like a helicopter circling it)
        Vector3 desiredPos = ball.position - (rotation * Vector3.forward * usedDistance);

        float smooth = (currentMode == CameraMode.FollowPOV)
            ? followSmooth
            : helicopterSmooth;

        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smooth * Time.deltaTime);

        transform.position = smoothedPos;
        transform.rotation = rotation;
    }
}
