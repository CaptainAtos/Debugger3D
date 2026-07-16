using System.Collections.Generic;
using UnityEngine;

public class BugSwarmManager : MonoBehaviour
{
    public static BugSwarmManager Instance;
    public List<BugAI> bugs = new List<BugAI>();

    // TODO (Zukunft): Wenn bugs.Count >= 50, alle despawnen und
    // einen "BugSwarmBoss" auf der Position spawnen.
    public int mergeThreshold = 50;

    void Awake()
    {
        Instance = this;
    }

    public void Register(BugAI bug)
    {
        bugs.Add(bug);
        CheckMerge();
    }

    public void Unregister(BugAI bug)
    {
        bugs.Remove(bug);
    }

    void CheckMerge()
    {
        if (bugs.Count >= mergeThreshold)
        {
            Debug.Log("Es ward ein Kakerlakenboss :D ");
        }
    }

    public Vector3 GetSwarmCenter(BugAI self, float radius)
    {
        Vector3 sum = Vector3.zero;
        int count = 0;
        for (int i = 0; i < bugs.Count; i++)
        {
            if (bugs[i] == self) continue;
            float dist = Vector3.Distance(bugs[i].transform.position, self.transform.position);
            if (dist <= radius)
            {
                sum += bugs[i].transform.position;
                count++;
            }
        }
        if (count == 0) return self.transform.position;
        return sum / count;
    }
}