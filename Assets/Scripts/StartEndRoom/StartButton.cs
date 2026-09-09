using UnityEngine;

public class StartButton : MonoBehaviour, IInteractable
{
    private RoundManager roundManager;

    void Start()
    {
        roundManager = FindFirstObjectByType<RoundManager>();
    }

    public bool IsInteractable
    {
        get { return true; }
    }

    public bool Interact()
    {
        roundManager.StartGame();
        return true;
    }
}