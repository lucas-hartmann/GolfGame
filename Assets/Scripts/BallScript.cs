using UnityEngine;

public class BallScript : MonoBehaviour
{
    [Header("Shot Settings")]
    [SerializeField] private float shotPowerMultiplier = 6f;
    [SerializeField] private float stopVelocity = 0.05f;
    [SerializeField] private float minShotPower = 0.3f;
    [SerializeField] private float maxVisualDistance = 3f;

    [Header("Keyboard Aim Settings")]
    [SerializeField] private float minPower = 0.5f;
    [SerializeField] private float maxPower = 3f;
    [SerializeField] private float powerSpeed = 1.0f;
    [SerializeField] private float angleSpeed = 60f;
    [SerializeField] private KeyCode shootKey = KeyCode.Space;
    [SerializeField] private KeyCode cancelKey = KeyCode.Escape;

    [Header("References")]
    [SerializeField] private LineRenderer lineRenderer;

    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager;

    private Vector3 lastSafePosition;
    private bool isIdle = false;
    private bool isAiming = false;

    private Rigidbody rb;

    private float currentPower;
    private float currentAngleDegrees;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError("LineForceKeyboard: Kein Rigidbody gefunden!");

        if (lineRenderer != null)
            lineRenderer.enabled = false;

        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
            if (gameManager == null)
                Debug.LogWarning("LineForceKeyboard: Kein GameManager gefunden. Setze die Referenz im Inspector, wenn nötig.");
        }

        currentPower = Mathf.Clamp((minPower + maxPower) * 0.5f, minPower, maxPower);
        currentAngleDegrees = 0f;
    }

    private void Start()
    {
        if (rb != null && rb.linearVelocity.magnitude < stopVelocity)
            isIdle = true;
    }

    private void Update()
    {
        if (rb != null && !isIdle && rb.linearVelocity.magnitude < stopVelocity)
            StopBall();

        if (isIdle && !isAiming)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                isAiming = true;
                if (lineRenderer != null) lineRenderer.enabled = true;
            }
        }

        if (isAiming && isIdle)
        {
            ProcessKeyboardAim();
        }

        if (isAiming && Input.GetKeyDown(cancelKey))
        {
            CancelAiming();
        }
    }

    private void ProcessKeyboardAim()
    {
        float angleDelta = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) angleDelta -= angleSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) angleDelta += angleSpeed * Time.deltaTime;
        currentAngleDegrees = Mathf.Repeat(currentAngleDegrees + angleDelta, 360f);

        float powerDelta = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) powerDelta += powerSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) powerDelta -= powerSpeed * Time.deltaTime;
        currentPower = Mathf.Clamp(currentPower + powerDelta, minPower, maxPower);

        DrawAimLine();

        if (Input.GetKeyDown(shootKey))
        {
            if (currentPower < minShotPower)
            {
                CancelAiming();
                return;
            }

            Vector3 dir = AngleToDirection(currentAngleDegrees);
            Vector3 targetOnBallHeight = transform.position + dir * currentPower;
            Shoot(targetOnBallHeight);

            // Plays the hit sound
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlayHitSound();
            }
        }
    }

    private Vector3 AngleToDirection(float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad)).normalized;
    }

    private void Shoot(Vector3 targetOnBallHeight)
    {
        lastSafePosition = transform.position;

        Vector3 diff = targetOnBallHeight - transform.position;
        float strength = diff.magnitude;
        strength = Mathf.Clamp(strength, 0f, maxVisualDistance);

        if (strength < minShotPower)
        {
            isAiming = false;
            if (lineRenderer != null) lineRenderer.enabled = false;
            return;
        }

        Vector3 direction = diff.normalized;

        isAiming = false;
        isIdle = false;
        if (lineRenderer != null) lineRenderer.enabled = false;

        rb.AddForce(direction * strength * shotPowerMultiplier, ForceMode.Impulse);

        if (gameManager != null)
        {
            gameManager.RegisterShot();
        }
    }

    private void DrawAimLine()
    {
        if (lineRenderer == null) return;

        Vector3 start = transform.position;
        Vector3 dir = AngleToDirection(currentAngleDegrees);
        float visualLength = Mathf.Clamp(currentPower, 0f, maxVisualDistance);
        Vector3 end = start + dir * visualLength;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.enabled = true;
    }

    private void StopBall()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        isIdle = true;

        if (gameManager != null && gameManager.GetCurrentShots() >= gameManager.maxShots)
        {
            gameManager.LoseGame();
        }
    }

    private void CancelAiming()
    {
        isAiming = false;
        if (lineRenderer != null) lineRenderer.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bounds"))
        {
            ResetToLastSafePosition();
        }
    }

    private void ResetToLastSafePosition()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = lastSafePosition;
        isIdle = true;
        CancelAiming();
    }
}
