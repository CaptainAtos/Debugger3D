using UnityEngine;

public class StartRoomExitTrigger : MonoBehaviour
{
    public static StartRoomExitTrigger Instance;

    private bool isArmed = false;

    private void Awake()
    {
        Instance = this;
    }

    public void Arm()
    {
        isArmed = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isArmed || !other.CompareTag("Player"))
        {
            return;
        }

        GameStateManager gameState = FindFirstObjectByType<GameStateManager>();
        gameState.ShowWinScreen();
    }
}
