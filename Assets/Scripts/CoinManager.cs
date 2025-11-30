using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public GameObject collectablePrefab;
    public Transform[] spawnPoints;

    [Min(1)]
    public int amountToSpawn = 5;
    private int coinsCollected = 0;

    void Start()
    {
        SpawnUnique();
    }

    public void SpawnUnique()
    {
        if (collectablePrefab == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("Prefab or Spawnpoints are missing!");
            return;
        }

        Transform[] points = (Transform[])spawnPoints.Clone();
        for (int i = points.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (points[i], points[j]) = (points[j], points[i]);
        }

        int count = Mathf.Min(amountToSpawn, points.Length);
        for (int i = 0; i < count; i++)
        {
            Instantiate(collectablePrefab, points[i].position, points[i].rotation);
        }
    }

    public void CoinCollected()
    {
        coinsCollected++;
        Debug.Log($"Coins collected: {coinsCollected}/{amountToSpawn}");

        // Optional Win Condition
        if (coinsCollected >= amountToSpawn)
        {
            Debug.Log("Alle Coins gesammelt!");
            // Hier später WinGame() / Tür öffnen / LevelComplete etc.
        }
    }
}
