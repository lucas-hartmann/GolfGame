using UnityEngine;

public class FlagWaver : MonoBehaviour
{
    [SerializeField] private float angle = 30f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;

    private float startRotation;

    private void Start()
    {
        startRotation = transform.localEulerAngles.y;
    }

    private void Update()
    {
        float rotationOffset = Mathf.Sin(Time.time * speed) * angle;

        transform.localRotation = Quaternion.Euler(0f, startRotation + rotationOffset, 0f);
    }
}

