using UnityEngine;
using UnityEngine.AI;

public class BugSpawner : MonoBehaviour
{
    public GameObject bugPrefab;
    public Transform player;

    public float spawnInterval = 5f;
    public float minSpawnDistance = 12f;
    public float maxSpawnDistance = 25f;
    public int maxBugs = 20;

    private float timer = 0f;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            TrySpawn();
        }
    }

    void TrySpawn()
    {
        Debug.Log("BugSpawner: TrySpawn called");

        if (BugSwarmManager.Instance != null && BugSwarmManager.Instance.bugs.Count >= maxBugs)
        {
            Debug.Log("BugSpawner: skipped, maxBugs reached");
            return;
        }

        Vector3 randomDir = Random.insideUnitSphere;
        randomDir.y = 0f;
        randomDir.Normalize();

        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
        Vector3 candidatePos = player.position + randomDir * distance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(candidatePos, out hit, 5f, NavMesh.AllAreas))
        {
            Debug.Log("BugSpawner: spawning bug at " + hit.position);
            Instantiate(bugPrefab, hit.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("BugSpawner: NavMesh.SamplePosition FAILED near " + candidatePos);
        }
    }
}