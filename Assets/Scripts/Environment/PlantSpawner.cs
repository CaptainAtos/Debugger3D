using UnityEngine;

public class PlantSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] plantPrefabs;
    [SerializeField] private float spawnChance = 0.4f;

    void Start()
    {
        float roll = Random.value;
        if (roll > spawnChance)
        {
            return;
        }

        int index = Random.Range(0, plantPrefabs.Length);
        Instantiate(plantPrefabs[index], transform.position, transform.rotation, transform);
    }
}
