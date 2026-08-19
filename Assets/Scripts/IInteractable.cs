using UnityEngine;

public interface IInteractable
{
    bool IsInteractable { get; }
    bool Interact();
}
