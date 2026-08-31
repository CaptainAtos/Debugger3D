using UnityEngine;

public class StartButton : MonoBehaviour, IInteractable
{
    [SerializeField] private RoundManager roundManager;

    public bool IsInteractable => true;

    public bool Interact()
    {
        roundManager.StartGame();
        return true;
    }
}