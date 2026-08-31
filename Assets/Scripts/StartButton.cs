using UnityEngine;

public class StartButton : MonoBehaviour, IInteractable
{
    [SerializeField] private EnergyFieldDoor door;

    public bool IsInteractable => true;

    public bool Interact()
    {
        door.Unlock();
        return true;
    }
}