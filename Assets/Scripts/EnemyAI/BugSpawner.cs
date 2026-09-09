using UnityEngine;

public class BugSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bugPrefab;
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

        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("CeilingSpawner");
        if (spawnPoints.Length == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, spawnPoints.Length);
        GameObject chosenSpawnPoint = spawnPoints[randomIndex];
        GameObject bugInstance = Instantiate(bugPrefab, chosenSpawnPoint.transform.position, Quaternion.identity);

        BugAI bugAI = bugInstance.GetComponent<BugAI>();
        if (bugAI != null)
            bugAI.Initialize(currentTier);
    }
}