using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BugSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bugPrefab;

    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxBugs = 20;
    [SerializeField] private string ceilingSpawnerName = "CeilingSpawner";

    //private float minSpawnDistance = 12f;
    //private float maxSpawnDistance = 25f;

    private Transform player;
    private float timer = 0f;

    void Start()
    {
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

        bool swarmIsFull = BugSwarmManager.Instance != null && BugSwarmManager.Instance.bugs.Count >= maxBugs;
        if (swarmIsFull)
        {
            Debug.Log("BugSpawner: skipped, maxBugs reached");
            return;
        }

        // Altes System: Spawn per NavMesh-Sampling in Spielernähe
        // Vector3 randomDir = Random.insideUnitSphere;
        // randomDir.y = 0f;
        // randomDir.Normalize();
        //
        // float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
        // Vector3 candidatePos = player.position + randomDir * distance;
        //
        // NavMeshHit hit;
        // if (NavMesh.SamplePosition(candidatePos, out hit, 5f, NavMesh.AllAreas))
        // {
        //     Debug.Log("BugSpawner: spawning bug at " + hit.position);
        //     Instantiate(bugPrefab, hit.position, Quaternion.identity);
        // }
        // else
        // {
        //     Debug.Log("BugSpawner: NavMesh.SamplePosition FAILED near " + candidatePos);
        // }

        // Neues System: Spawn an Decken-Spawnpunkten, Bug fällt danach runter
        List<Transform> spawnPoints = FindCeilingSpawnPoints();

        if (spawnPoints.Count == 0)
        {
            Debug.Log("BugSpawner: keine CeilingSpawner-Objekte gefunden");
            return;
        }

        int randomIndex = Random.Range(0, spawnPoints.Count);
        Transform chosenSpawnPoint = spawnPoints[randomIndex];

        Debug.Log("BugSpawner: spawne Bug an Deckenpunkt " + chosenSpawnPoint.position);
        Instantiate(bugPrefab, chosenSpawnPoint.position, Quaternion.identity);
    }

    List<Transform> FindCeilingSpawnPoints()
    {
        Transform[] allTransforms = FindObjectsByType<Transform>(FindObjectsSortMode.None);
        List<Transform> spawnPoints = new List<Transform>();

        for (int i = 0; i < allTransforms.Length; i++)
        {
            if (allTransforms[i].name.Contains(ceilingSpawnerName))
            {
                spawnPoints.Add(allTransforms[i]);
            }
        }

        return spawnPoints;
    }
}
