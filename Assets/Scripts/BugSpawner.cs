using System.Collections.Generic;
using UnityEngine;

public class BugSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bugPrefab;
    [SerializeField] private string ceilingSpawnerName = "CeilingSpawner";
    [SerializeField] private BugDifficultyTier[] tiers;

    private float timer = 0f;
    private bool isSpawning = false;
    private BugDifficultyTier currentTier;

    void Update()
    {
        if (!isSpawning) return;

        timer += Time.deltaTime;
        if (timer >= currentTier.spawnInterval)
        {
            timer = 0f;
            TrySpawn();
        }
    }

    public void StartSpawning(int tierIndex)
    {
        if (tierIndex < 0 || tierIndex >= tiers.Length)
        {
            Debug.LogWarning("BugSpawner: ungültiger Tier-Index " + tierIndex);
            return;
        }

        currentTier = tiers[tierIndex];
        timer = 0f;
        isSpawning = true;
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    void TrySpawn()
    {
        bool swarmIsFull = BugSwarmManager.Instance != null && BugSwarmManager.Instance.bugs.Count >= currentTier.maxBugs;
        if (swarmIsFull)
            return;

        List<Transform> spawnPoints = FindCeilingSpawnPoints();
        if (spawnPoints.Count == 0)
        {
            Debug.Log("BugSpawner: keine CeilingSpawner-Objekte gefunden");
            return;
        }

        Transform chosenSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject bugInstance = Instantiate(bugPrefab, chosenSpawnPoint.position, Quaternion.identity);

        BugAI bugAI = bugInstance.GetComponent<BugAI>();
        if (bugAI != null)
            bugAI.Initialize(currentTier);
    }

    List<Transform> FindCeilingSpawnPoints()
    {
        Transform[] allTransforms = FindObjectsByType<Transform>(FindObjectsSortMode.None);
        List<Transform> spawnPoints = new List<Transform>();

        for (int i = 0; i < allTransforms.Length; i++)
        {
            if (allTransforms[i].name.Contains(ceilingSpawnerName))
                spawnPoints.Add(allTransforms[i]);
        }

        return spawnPoints;
    }
}