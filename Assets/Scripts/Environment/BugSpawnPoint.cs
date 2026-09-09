using UnityEngine;

public class BugSpawnPoint : MonoBehaviour
{
    [SerializeField] private Vector3 boundsHalfExtents = new Vector3(4f, 0f, 4f);

    public Vector3 BoundsHalfExtents
    {
        get { return boundsHalfExtents; }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 size = new Vector3(boundsHalfExtents.x * 2f, 0.1f, boundsHalfExtents.z * 2f);
        Gizmos.DrawWireCube(transform.position, size);
    }
}
