using UnityEngine;
using TMPro;

public class RoundManager : MonoBehaviour
{
    [SerializeField] private ServerSpawner serverSpawner;
    [SerializeField] private BugSpawner bugSpawner;
    [SerializeField] private SelfDefenseSystem defenseSystem;

    [SerializeField] private float switchTimerDuration = 60f;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private AudioSource timerAudio;

    private EnergyFieldDoor door;
    private LampDisplay lampDisplay;
    private PowerResetSwitch powerSwitch;

    private int currentRound = 1;
    private int serversActiveThisRound = 0;
    private int serversRequiredThisRound = 3;

    private float switchTimer = 0f;
    private bool switchTimerRunning = false;

    public void Initialize(EnergyFieldDoor spawnedDoor, LampDisplay spawnedLampDisplay, PowerResetSwitch spawnedPowerSwitch)
    {
        door = spawnedDoor;
        lampDisplay = spawnedLampDisplay;
        powerSwitch = spawnedPowerSwitch;
    }

    void Start()
    {
        timerText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!switchTimerRunning) return;

        switchTimer -= Time.deltaTime;
        int secondsLeft = Mathf.CeilToInt(switchTimer);
        timerText.text = "Schalter gesperrt: " + secondsLeft + " s";

        if (switchTimer <= 0f)
        {
            switchTimerRunning = false;
            timerText.gameObject.SetActive(false);
            powerSwitch.ResetLever();
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
        serverSpawner.PlayOutageSparks();

        CeilingLampFlicker[] lamps = FindObjectsByType<CeilingLampFlicker>(FindObjectsSortMode.None);
        for (int i = 0; i < lamps.Length; i++)
        {
            lamps[i].PlayFlicker();
        }

        if (StartRoomExitTrigger.Instance != null)
        {
            StartRoomExitTrigger.Instance.Arm();
        }

        if (currentRound < 3)
        {
            powerSwitch.ResetLever();
        }
        else
        {
            StartSwitchTimer();
        }
    }

    private void StartSwitchTimer()
    {
        switchTimer = switchTimerDuration;
        switchTimerRunning = true;
        timerText.gameObject.SetActive(true);

        if (timerAudio != null)
        {
            timerAudio.Play();
        }
    }

    public void OnSwitchPressed()
    {
        serverSpawner.StopOutageSparks();

        CeilingLampFlicker[] lamps = FindObjectsByType<CeilingLampFlicker>(FindObjectsSortMode.None);
        for (int i = 0; i < lamps.Length; i++)
        {
            lamps[i].StopFlicker();
        }

        lampDisplay.SetRoundComplete(currentRound - 1);

        if (currentRound < 3)
        {
            currentRound++;
            serversActiveThisRound = 0;
            if (currentRound == 2)
            {
                serversRequiredThisRound = 6;
            }
            else
            {
                serversRequiredThisRound = 9;
            }
            serverSpawner.SpawnServers(serversRequiredThisRound);
        }
        else
        {
            defenseSystem.Trigger();
            door.Unlock();
        }
    }
}
