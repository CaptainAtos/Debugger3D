using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField] private LampDisplay lampDisplay;

    [SerializeField] private ServerSpawner serverSpawner;
    [SerializeField] private BugSpawner bugSpawner;
    [SerializeField] private EnergyFieldDoor door;
    [SerializeField] private SelfDefenseSystem defenseSystem;

    [SerializeField] private PowerResetSwitch powerSwitch;

    private int currentRound = 1;
    private int serversActiveThisRound = 0;
    private int serversRequiredThisRound = 3;

    public void Initialize(EnergyFieldDoor spawnedDoor, LampDisplay spawnedLampDisplay, PowerResetSwitch spawnedPowerSwitch)
    {
        door = spawnedDoor;
        lampDisplay = spawnedLampDisplay;
        powerSwitch = spawnedPowerSwitch;

        if (powerSwitch == null)
        {
            powerSwitch = FindFirstObjectByType<PowerResetSwitch>();
        }
    }

    public void StartGame()
    {
        door.Unlock();
        serverSpawner.SpawnServers(serversRequiredThisRound);
    }

    public void OnServerActivated()
    {
        serversActiveThisRound++;

        if (serversActiveThisRound >= serversRequiredThisRound)
        {
            TriggerPowerOutage();
        }
    }

    private void TriggerPowerOutage()
    {
        door.Lock();
        bugSpawner.StartSpawning(currentRound - 1);
        powerSwitch.ResetLever();
        CeilingLampFlicker.FlickerAll();

        if (StartRoomExitTrigger.Instance != null)
        {
            StartRoomExitTrigger.Instance.Arm();
        }

        if (currentRound == 3)
        {
            defenseSystem.Trigger();
        }
    }

    public void OnSwitchPressed()
    {
        CeilingLampFlicker.StopAll();
        lampDisplay.SetRoundComplete(currentRound - 1);

        if (currentRound < 3)
        {
            currentRound++;
            serversActiveThisRound = 0;
            serversRequiredThisRound = currentRound == 2 ? 6 : 9;
            serverSpawner.SpawnServers(serversRequiredThisRound);
        }
        else
        {
            door.Unlock();
        }
    }
}