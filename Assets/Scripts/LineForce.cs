using UnityEngine;
using TMPro;

public class LineForce : MonoBehaviour
{
    [SerializeField] private float shotPower;
    [SerializeField] private float stopVelocity = .05f;
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private ScoreSystem scoreSystem;
    [SerializeField] private SoundManager soundManager;

    private Vector3 lastSafePosition;

    private bool isIdle;
    private bool isAiming;

    private Rigidbody rigidbody;

    private void Awake() {

        rigidbody = GetComponent<Rigidbody>();

        isAiming = false;
        lineRenderer.enabled = false;
    }

    private void Update() {

        if (rigidbody.linearVelocity.magnitude < stopVelocity) {
            Stop();
        }

        ProcessAim();
    }

    private void OnMouseDown() {
        if (isIdle) {
            isAiming = true;
        }
    }

    private void ProcessAim() {
        if(!isAiming || !isIdle){
            return;
        }

        Vector3? worldPoint = CastMouseClickRay();

        if (!worldPoint.HasValue) {
            return;
        }

        DrawLine(worldPoint.Value);

        if(Input.GetMouseButtonUp(0)){
            Shoot(worldPoint.Value);
        }
    }

    private void Shoot(Vector3 worldPoint){
        lastSafePosition = transform.position;

        isAiming = false;
        lineRenderer.enabled = false;

        Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, transform.position.y, worldPoint.z);

        Vector3 direction = (horizontalWorldPoint - transform.position).normalized;
        float strength = Vector3.Distance(transform.position, horizontalWorldPoint);

        rigidbody.AddForce(direction * strength * shotPower);
        scoreSystem.AddStroke(1);
        soundManager.ShotSound();
        //isIdle = false;
    }

    private void DrawLine(Vector3 worldPoint) {
        Vector3[] positions = {transform.position, worldPoint};
        lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, worldPoint);
            lineRenderer.enabled = true;
    }

    private void Stop(){
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        isIdle = true; 
    }

    private Vector3? CastMouseClickRay() {
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);

        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);

        RaycastHit hit;
        if (Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, Mathf.Infinity)) {
            return hit.point;
        } else {
            return null;
        }
    }
    
    //OutOfBounds stuff
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
