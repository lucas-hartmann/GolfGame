using UnityEngine;

public class CoinAnimation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float floatAmplitude = 0.25f;
    [SerializeField] private float floatFrequency = 2f;

    [SerializeField] private ScoreSystem scoreSystem;
    [SerializeField] private SoundManager soundManager;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        RotateCoin();
        FloatCoin();
    }

    private void RotateCoin()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    private void FloatCoin()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                soundManager.CoinSound();
                Destroy(gameObject);
                scoreSystem.AddScore(100);
            }
        }
}
