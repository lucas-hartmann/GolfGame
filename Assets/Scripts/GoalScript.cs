using UnityEngine;
using System.Collections;

public class GoalScript : MonoBehaviour
{
    [SerializeField] private GameManager GameManager;

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
            StartCoroutine(WinAfterParticles());
             col.enabled = false;
        }
    }

    private IEnumerator WinAfterParticles()
    {
        ParticleSystem ps = Instantiate(particlePrefab, confettiPos.position, confettiPos.rotation);
        yield return new WaitForSeconds(2);
        GameManager.WinGame();
    }
}
