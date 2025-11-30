using UnityEngine;

public class GoalScript : MonoBehaviour
{
    [SerializeField] private GameManager GameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.WinGame();
        }
    }
}
