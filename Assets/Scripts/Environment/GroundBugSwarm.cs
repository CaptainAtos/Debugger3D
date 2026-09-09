using System.Threading.Tasks;
using UnityEngine;

public class GroundBugSwarm : MonoBehaviour
{
    [SerializeField] private bool useMultithreading = true;
    [SerializeField] private GameObject bugPrefab;
    [SerializeField] private int bugsPerSpawnPoint = 150;
    [SerializeField] private float neighborRadius = 1f;
    [SerializeField] private float moveSpeed = 1.5f;

    [Range(0f, 5f)]
    [SerializeField] private float separationWeight = 1.5f;

    [Range(0f, 5f)]
    [SerializeField] private float alignmentWeight = 1f;

    [Range(0f, 5f)]
    [SerializeField] private float cohesionWeight = 1f;

    [SerializeField] private float containmentWeight = 3f;

    [SerializeField] private float playerFleeRadius = 3f;

    [Range(0f, 5f)]
    [SerializeField] private float fleeWeight = 2f;

    private Transform player;
    [SerializeField] private int bugCount;
    private Transform[] bugTransforms;
    private Vector3[] positions;
    private Vector3[] velocities;
    private Vector3[] newPositions;
    private Vector3[] newVelocities;
    private Vector3[] zoneCenters;
    private Vector3[] zoneBounds;
    private int[] zoneIndex;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        BugSpawnPoint[] spawnPoints = FindObjectsByType<BugSpawnPoint>(FindObjectsSortMode.None);
        bugCount = spawnPoints.Length * bugsPerSpawnPoint;

        bugTransforms = new Transform[bugCount];
        positions = new Vector3[bugCount];
        velocities = new Vector3[bugCount];
        newPositions = new Vector3[bugCount];
        newVelocities = new Vector3[bugCount];
        zoneCenters = new Vector3[bugCount];
        zoneBounds = new Vector3[bugCount];
        zoneIndex = new int[bugCount];

        int index = 0;

        for (int s = 0; s < spawnPoints.Length; s++)
        {
            Vector3 center = spawnPoints[s].transform.position;
            Vector3 bounds = spawnPoints[s].BoundsHalfExtents;

            for (int b = 0; b < bugsPerSpawnPoint; b++)
            {
                float offsetX = Random.Range(-bounds.x, bounds.x);
                float offsetZ = Random.Range(-bounds.z, bounds.z);
                Vector3 spawnPos = center + new Vector3(offsetX, 0f, offsetZ);

                GameObject bug = Instantiate(bugPrefab, spawnPos, Quaternion.identity, transform);

                bugTransforms[index] = bug.transform;
                positions[index] = spawnPos;
                zoneCenters[index] = center;
                zoneBounds[index] = bounds;
                zoneIndex[index] = s;

                Vector2 randomDir = Random.insideUnitCircle.normalized;
                velocities[index] = new Vector3(randomDir.x, 0f, randomDir.y) * moveSpeed;

                index++;
            }
        }
    }

    void Update()
    {
        float dt = Time.deltaTime;
        Vector3 playerPos = player.position;

        if (useMultithreading)
        {
            Parallel.For(0, bugCount, i =>
            {
                ComputeBugMovement(i, dt, playerPos);
            });
        }
        else
        {
            for (int i = 0; i < bugCount; i++)
            {
                ComputeBugMovement(i, dt, playerPos);
            }
        }

        for (int i = 0; i < bugCount; i++)
        {
            positions[i] = newPositions[i];
            velocities[i] = newVelocities[i];
            bugTransforms[i].position = positions[i];
        }
    }

    private void ComputeBugMovement(int i, float dt, Vector3 playerPos)
    {
        Vector3 separation = Vector3.zero;
        Vector3 alignment = Vector3.zero;
        Vector3 cohesion = Vector3.zero;
        int neighborCount = 0;

        for (int j = 0; j < bugCount; j++)
        {
            if (j == i) continue;
            if (zoneIndex[j] != zoneIndex[i]) continue;

            float distance = Vector3.Distance(positions[i], positions[j]);
            if (distance < neighborRadius)
            {
                separation += (positions[i] - positions[j]) / Mathf.Max(distance, 0.01f);
                alignment += velocities[j];
                cohesion += positions[j];
                neighborCount++;
            }
        }

        Vector3 direction = velocities[i];

        if (neighborCount > 0)
        {
            alignment /= neighborCount;
            cohesion = (cohesion / neighborCount) - positions[i];

            direction += separation * separationWeight * dt;
            direction += alignment * alignmentWeight * dt;
            direction += cohesion * cohesionWeight * dt;
        }

        Vector3 fromPlayer = positions[i] - playerPos;
        float playerDistance = fromPlayer.magnitude;
        if (playerDistance < playerFleeRadius)
        {
            Vector3 fleeDirection = fromPlayer / Mathf.Max(playerDistance, 0.01f);
            direction += fleeDirection * fleeWeight * dt;
        }

        Vector3 center = zoneCenters[i];
        Vector3 bounds = zoneBounds[i];
        Vector3 localPos = positions[i] - center;
        Vector3 pull = Vector3.zero;

        if (localPos.x > bounds.x) pull.x = -1f;
        else if (localPos.x < -bounds.x) pull.x = 1f;

        if (localPos.z > bounds.z) pull.z = -1f;
        else if (localPos.z < -bounds.z) pull.z = 1f;

        if (pull != Vector3.zero)
        {
            direction += pull.normalized * containmentWeight * dt;
        }

        direction.y = 0f;
        direction = direction.normalized * moveSpeed;

        newVelocities[i] = direction;

        Vector3 nextPos = positions[i] + direction * dt;
        nextPos.y = positions[i].y;
        newPositions[i] = nextPos;
    }
}
