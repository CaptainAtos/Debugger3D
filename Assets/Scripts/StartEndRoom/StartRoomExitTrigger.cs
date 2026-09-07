using UnityEngine;

public class StartRoomExitTrigger : MonoBehaviour
{
    public static StartRoomExitTrigger Instance { get; private set; }

    private bool isArmed = false;

    public event System.Action OnWin;

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

        if (SelfDefenseSystem.Instance != null)
        {
            SelfDefenseSystem.Instance.PlayerReachedExit();
        }

        OnWin?.Invoke();
    }
}