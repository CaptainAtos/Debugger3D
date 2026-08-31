using UnityEngine;

public class EnergyFieldDoor : MonoBehaviour
{
    [SerializeField] private GameObject fieldBarrier; 

    public void Lock()
    {
        fieldBarrier.SetActive(true);
    }

    public void Unlock()
    {
        fieldBarrier.SetActive(false);
    }
}