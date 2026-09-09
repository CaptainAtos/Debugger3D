using System.Collections.Generic;
using UnityEngine;

public class BugSwarmManager : MonoBehaviour
{
    public static BugSwarmManager Instance;
    public List<BugAI> bugs = new List<BugAI>();

    void Awake()
    {
        Instance = this;
    }

    public void Register(BugAI bug)
    {
        bugs.Add(bug);
    }

    public void Unregister(BugAI bug)
    {
        bugs.Remove(bug);
    }

    public Vector3 GetSwarmCenter(BugAI self, float radius)
    {
        Vector3 sum = Vector3.zero;
        int count = 0;
        for (int i = 0; i < bugs.Count; i++)
        {
            if (bugs[i] == self)
                continue;
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