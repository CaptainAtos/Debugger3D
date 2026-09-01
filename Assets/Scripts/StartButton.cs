using UnityEngine;

public class StartButton : MonoBehaviour, IInteractable
{
    [SerializeField] private RoundManager roundManager;

    void Start()
    {
        if (roundManager == null)
        {
            roundManager = FindFirstObjectByType<RoundManager>();
        }
    }

    public bool IsInteractable => true;

    public bool Interact()
    {
        roundManager.StartGame();
        return true;
    }
}