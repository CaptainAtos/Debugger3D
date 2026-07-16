using UnityEngine;
using System.Collections.Generic;
using Unity.AI.Navigation;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject roomEmpty;
    public GameObject gangH;
    public GameObject gangV;
    public GameObject eckNE;
    public GameObject eckNW;
    public GameObject eckSE;
    public GameObject eckSW;
    public GameObject wallNorth;
    public GameObject wallSouth;
    public GameObject wallEast;
    public GameObject wallWest;
    public GameObject wallPrefab;

    public int maxRooms = 20;

    private List<GameObject> allRooms = new List<GameObject>();
    private List<GameObject> placedRooms = new List<GameObject>();
    private List<Transform> openConnectors = new List<Transform>();

    void Start()
    {
        allRooms.Add(roomEmpty);
        allRooms.Add(gangH);
        allRooms.Add(gangV);
        allRooms.Add(eckNE);
        allRooms.Add(eckNW);
        allRooms.Add(eckSE);
        allRooms.Add(eckSW);
        allRooms.Add(wallNorth);
        allRooms.Add(wallSouth);
        allRooms.Add(wallEast);
        allRooms.Add(wallWest);

        GameObject start = Instantiate(roomEmpty, Vector3.zero, Quaternion.identity);
        AddRoom(start);

        BuildDungeon();
        BuildWalls();

        NavMeshSurface surface = GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
    }

    void BuildDungeon()
    {
        while (openConnectors.Count > 0 && placedRooms.Count < maxRooms)
        {
            Transform conn = openConnectors[0];
            openConnectors.RemoveAt(0);

            if (IsConnected(conn))
            {
                continue;
            }

            string needed = Opposite(conn.name);

            List<GameObject> fitting = new List<GameObject>();
            for (int i = 0; i < allRooms.Count; i++)
            {
                if (HasConnector(allRooms[i], needed))
                {
                    fitting.Add(allRooms[i]);
                }
            }

            for (int i = 0; i < fitting.Count; i++)
            {
                int randomIndex = Random.Range(0, fitting.Count);
                GameObject prefab = fitting[randomIndex];

                GameObject newRoom = Instantiate(prefab);
                Transform match = GetConnector(newRoom, needed);

                Vector3 move = conn.position - match.position;
                newRoom.transform.position = newRoom.transform.position + move;

                if (RoomOverlaps(newRoom))
                {
                    Destroy(newRoom);
                }
                else
                {
                    AddRoom(newRoom);
                    break;
                }
            }
        }
    }

    void AddRoom(GameObject room)
    {
        placedRooms.Add(room);

        for (int i = 0; i < room.transform.childCount; i++)
        {
            Transform child = room.transform.GetChild(i);
            if (child.name.Contains("Connector"))
            {
                openConnectors.Add(child);
            }
        }
    }

    bool RoomOverlaps(GameObject room)
    {
        Vector2 pos = RoundPos(room.transform.position);
        for (int i = 0; i < placedRooms.Count; i++)
        {
            if (RoundPos(placedRooms[i].transform.position) == pos)
            {
                return true;
            }
        }
        return false;
    }

    bool IsConnected(Transform conn)
    {
        Vector2 pos = RoundPos(conn.position);
        int count = 0;

        for (int i = 0; i < placedRooms.Count; i++)
        {
            GameObject room = placedRooms[i];
            for (int j = 0; j < room.transform.childCount; j++)
            {
                Transform child = room.transform.GetChild(j);
                if (child.name.Contains("Connector") && RoundPos(child.position) == pos)
                {
                    count++;
                }
            }
        }

        return count > 1;
    }

    void BuildWalls()
    {
        for (int i = 0; i < placedRooms.Count; i++)
        {
            GameObject room = placedRooms[i];
            for (int j = 0; j < room.transform.childCount; j++)
            {
                Transform child = room.transform.GetChild(j);
                if (child.name.Contains("Connector") && !IsConnected(child))
                {
                    Quaternion rot = Quaternion.identity;
                    if (child.name.Contains("East") || child.name.Contains("West"))
                    {
                        rot = Quaternion.Euler(0, 90, 0);
                    }
                    Vector3 wallPos = child.position;
                    wallPos.y = 2.5f;
                    Instantiate(wallPrefab, wallPos, rot);
                }
            }
        }
    }

    Transform GetConnector(GameObject room, string direction)
    {
        for (int i = 0; i < room.transform.childCount; i++)
        {
            Transform child = room.transform.GetChild(i);
            if (child.name.Contains("Connector") && child.name.Contains(direction))
            {
                return child;
            }
        }
        return null;
    }

    bool HasConnector(GameObject prefab, string direction)
    {
        for (int i = 0; i < prefab.transform.childCount; i++)
        {
            Transform child = prefab.transform.GetChild(i);
            if (child.name.Contains("Connector") && child.name.Contains(direction))
            {
                return true;
            }
        }
        return false;
    }

    string Opposite(string name)
    {
        if (name.Contains("North")) return "South";
        if (name.Contains("South")) return "North";
        if (name.Contains("East")) return "West";
        if (name.Contains("West")) return "East";
        return "";
    }

    Vector2 RoundPos(Vector3 pos)
    {
        float x = Mathf.Round(pos.x * 10f) / 10f;
        float z = Mathf.Round(pos.z * 10f) / 10f;
        return new Vector2(x, z);
    }
}