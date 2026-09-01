using System.Collections.Generic;
using UnityEngine;

public class ServerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject serverPrefab;
    [SerializeField] private RoundManager roundManager;

    private List<GameObject> activeServers = new List<GameObject>();

    public void SpawnServers(int count)
    {
        ClearCurrentServers();

        GameObject[] allSpawnPoints = GameObject.FindGameObjectsWithTag("ServerSpawnPoint");
        List<GameObject> available = new List<GameObject>(allSpawnPoints);

        for (int i = 0; i < count && available.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, available.Count);
            GameObject chosenPoint = available[randomIndex];
            available.RemoveAt(randomIndex);

            Quaternion spawnRotation = chosenPoint.transform.rotation * Quaternion.Euler(0f, 180f, 0f);
            GameObject serverInstance = Instantiate(serverPrefab, chosenPoint.transform.position, spawnRotation);
            activeServers.Add(serverInstance);

            ServerLEDController led = serverInstance.GetComponentInChildren<ServerLEDController>();
            if (led != null)
            {
                led.OnActivated += HandleServerActivated;
            }
        }
    }

    private void HandleServerActivated()
    {
        roundManager.OnServerActivated();
    }

    private void ClearCurrentServers()
    {
        foreach (GameObject server in activeServers)
        {
            if (server != null)
                Destroy(server);
        }
        activeServers.Clear();
    }
}