using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
            UnlockNewLevel();
            StartCoroutine(WinAfterParticles());
             col.enabled = false;
        }
    }

    private IEnumerator WinAfterParticles()
    {
        ParticleSystem ps = Instantiate(particlePrefab, confettiPos.position, confettiPos.rotation);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayWinSound();
        }
        yield return new WaitForSeconds(2);
        GameManager.WinGame();
    }

    void UnlockNewLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }
}
