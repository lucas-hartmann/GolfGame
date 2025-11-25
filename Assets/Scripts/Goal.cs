using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private ScoreSystem scoreSystem;
    [SerializeField] private SoundManager soundManager;

    [SerializeField] private ParticleSystem particlePrefab;
    [SerializeField] private Transform confettiPos;

    private Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            col.enabled = false;
            Instantiate(particlePrefab, confettiPos.position, confettiPos.rotation);
            soundManager.GoalSound();
            scoreSystem.EndGame();
        }
    }
}

