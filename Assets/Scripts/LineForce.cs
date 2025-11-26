using UnityEngine;

public class LineForce : MonoBehaviour
{
    [Header("Shot Settings")]
    [SerializeField] private float shotPower = 6f;
    [SerializeField] private float stopVelocity = 0.05f;
    [SerializeField] private float minDragDistance = 0.3f;
    [SerializeField] private float maxDragDistance = 3f;

    [Header("References")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private ScoreSystem scoreSystem;
    [SerializeField] private Camera aimCamera;
    [SerializeField] private LayerMask aimLayerMask = ~0;

    private Vector3 lastSafePosition;

    private bool isIdle;
    private bool isAiming;

    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

        if (aimCamera == null)
        {
            aimCamera = Camera.main;
        }

        isAiming = false;
        isIdle = false;

        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }

    private void Start()
    {
        // At start the ball is usually not moving, so mark it idle
        if (rigidbody.linearVelocity.magnitude < stopVelocity)
        {
            isIdle = true;
        }
    }

    private void Update()
    {
        // When the ball slows down enough, mark it idle again
        if (!isIdle && rigidbody.linearVelocity.magnitude < stopVelocity)
        {
            Stop();
        }

        // Start aiming by clicking anywhere on the screen if the ball is idle
        if (isIdle && Input.GetMouseButtonDown(0))
        {
            isAiming = true;
        }

        ProcessAim();
    }

    private void ProcessAim()
    {
        if (!isAiming || !isIdle)
            return;

        Vector3? hitPointNullable = CastMouseClickRay();
        if (!hitPointNullable.HasValue)
            return;

        Vector3 hitPoint = hitPointNullable.Value;

        // Point on the same height as the ball (used for physics)
        Vector3 targetOnBallHeight = new Vector3(hitPoint.x, transform.position.y, hitPoint.z);

        // Draw line exactly to the raycast hit (so it matches the cursor)
        DrawLine(hitPoint);

        if (Input.GetMouseButtonUp(0))
        {
            Shoot(targetOnBallHeight);
        }
    }

    private void Shoot(Vector3 targetOnBallHeight)
    {
        lastSafePosition = transform.position;

        Vector3 diff = targetOnBallHeight - transform.position;
        float strength = diff.magnitude;

        // Clamp drag distance so you cannot over-power it
        strength = Mathf.Clamp(strength, 0f, maxDragDistance);

        // Minimum drag check: treat tiny drags as "no shot"
        if (strength < minDragDistance)
        {
            isAiming = false;
            if (lineRenderer != null) lineRenderer.enabled = false;
            return;
        }

        Vector3 direction = diff.normalized;

        isAiming = false;
        isIdle = false; // lock shooting while ball is moving
        if (lineRenderer != null) lineRenderer.enabled = false;

        // Same AddForce style as before (default Force mode)
        rigidbody.AddForce(direction * strength * shotPower);
        scoreSystem.AddStroke(1);
    }

    private void DrawLine(Vector3 worldPoint)
    {
        if (lineRenderer == null) return;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, worldPoint);
        lineRenderer.enabled = true;
    }

    private void Stop()
    {
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        isIdle = true;
    }

    private Vector3? CastMouseClickRay()
    {
        if (aimCamera == null)
            return null;

        Ray ray = aimCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, aimLayerMask))
        {
            return hit.point;
        }

        return null;
    }

    // Out-of-bounds stuff
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bounds"))
        {
            ResetToLastSafePosition();
        }
    }

    private void ResetToLastSafePosition()
    {
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        transform.position = lastSafePosition;
        isIdle = true;
    }
}
