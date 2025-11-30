using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float floatAmplitude = 0.25f;
    [SerializeField] private float floatFrequency = 2f;

    [SerializeField] private GameManager gameManager;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
            Debug.LogError("Kein GameManager im Level gefunden!");
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
            if (gameManager != null)
            {
                gameManager.CoinCollected();
            }

            Destroy(gameObject);
        }
    }
}
