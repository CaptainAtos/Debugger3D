using UnityEngine;

public class SelfDefenseSystem : MonoBehaviour
{
    [SerializeField] private BugSpawner bugSpawner;
    [SerializeField] private GroundBugSwarm groundBugSwarm;

    public void Trigger()
    {
        bugSpawner.StopSpawning();

        BugAI[] bugs = FindObjectsByType<BugAI>(FindObjectsSortMode.None);
        for (int i = 0; i < bugs.Length; i++)
        {
            bugs[i].Die();
        }

        Destroy(groundBugSwarm.gameObject);
    }
}
