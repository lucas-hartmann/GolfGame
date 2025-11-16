using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private ScoreSystem scoreSystem;

    private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                scoreSystem.EndGame();
            }
        }
}
