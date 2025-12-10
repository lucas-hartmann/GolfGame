using UnityEngine;

public class CameraRigController : MonoBehaviour
{
    [Header("Target")]
    public Transform ball;

    // NEW: Assign an empty GameObject here to define the fixed view position/rotation
    [Header("Fixed Helicopter View")]
    public Transform fixedHelicopterView;

    public enum CameraMode { FollowPOV, Helicopter }
    [Header("Mode")]
    public CameraMode currentMode = CameraMode.FollowPOV;

    [Header("Close View Settings")]
    public float followPitchMin = 5f;
    public float followPitchMax = 60f;
    public float followDistance = 7f;
    public float followMinDistance = 4f;
    public float followMaxDistance = 12f;

    [Header("Shared Settings")]
    public float rotateSpeed = 120f;
    public float zoomSpeed = 5f;
    public float followSmooth = 10f;
    public float helicopterSmooth = 5f; // Speed of moving to the fixed spot

    private float yaw;
    private float pitch;

    void Start()
    {
        if (!ball) return;

        // Initialize yaw & pitch from current rig orientation relative to ball
        Vector3 dir = (transform.position - ball.position).normalized;
        pitch = Mathf.Asin(dir.y) * Mathf.Rad2Deg;
        yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        pitch = Mathf.Clamp(pitch, followPitchMin, followPitchMax);
    }

    void LateUpdate()
    {
        if (!ball) return;

        // ----- SWITCH MODE (C) -----
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Toggle between the two modes
            currentMode = (currentMode == CameraMode.FollowPOV)
                ? CameraMode.Helicopter
                : CameraMode.FollowPOV;
        }

        // Variables to store where the camera WANTS to be this frame
        Vector3 targetPosition;
        Quaternion targetRotation;
        float currentSmoothSpeed;

        // ----- CALCULATE BASED ON MODE -----
        if (currentMode == CameraMode.FollowPOV)
        {
            // 1. Handle Rotation Input (Only in POV)
            if (Input.GetMouseButton(1))
            {
                yaw += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
                pitch -= Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
                pitch = Mathf.Clamp(pitch, followPitchMin, followPitchMax);
            }

            // 2. Handle Zoom Input (Only in POV)
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.0001f)
            {
                followDistance -= scroll * zoomSpeed;
                followDistance = Mathf.Clamp(followDistance, followMinDistance, followMaxDistance);
            }

            // 3. Calculate Target Spot (Orbiting the ball)
            targetRotation = Quaternion.Euler(pitch, yaw, 0f);
            targetPosition = ball.position - (targetRotation * Vector3.forward * followDistance);
            currentSmoothSpeed = followSmooth;
        }
        else // Helicopter Mode (Fixed)
        {
            // In this mode, we do NOT touch yaw/pitch or read inputs.
            // We strictly fly to the fixed transform.
            if (fixedHelicopterView != null)
            {
                targetPosition = fixedHelicopterView.position;
                targetRotation = fixedHelicopterView.rotation;
            }
            else
            {
                // Fallback if you forgot to assign the transform
                targetPosition = transform.position;
                targetRotation = transform.rotation;
                Debug.LogWarning("Assign a Fixed Helicopter View Transform in the Inspector!");
            }
            currentSmoothSpeed = helicopterSmooth;
        }

        // ----- APPLY MOVEMENT -----
        // Smoothly move/rotate the camera rig to the calculated target
        transform.position = Vector3.Lerp(transform.position, targetPosition, currentSmoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentSmoothSpeed * Time.deltaTime);
    }
}