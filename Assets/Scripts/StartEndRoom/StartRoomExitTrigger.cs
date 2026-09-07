using UnityEngine;

public class StartRoomExitTrigger : MonoBehaviour
{
    public static StartRoomExitTrigger Instance { get; private set; }

    private bool isArmed = false;

    public event System.Action OnWin;

    private void Awake()
    {
        Instance = this;
        Debug.Log("StartRoomExitTrigger Awake auf " + gameObject.name + " (InstanceID " + GetInstanceID() + ")");
    }

    public void Arm()
    {
        isArmed = true;
        Debug.Log("StartRoomExitTrigger.Arm() aufgerufen auf " + gameObject.name + " (InstanceID " + GetInstanceID() + ")");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("StartRoomExitTrigger.OnTriggerEnter mit " + other.name + " (Tag: " + other.tag + "), isArmed = " + isArmed + " auf " + gameObject.name);

        if (!isArmed || !other.CompareTag("Player"))
        {
            return;
        }

        if (SelfDefenseSystem.Instance != null)
        {
            SelfDefenseSystem.Instance.PlayerReachedExit();
        }

        Debug.Log("StartRoomExitTrigger: OnWin wird jetzt ausgelöst");
        OnWin?.Invoke();
    }
}