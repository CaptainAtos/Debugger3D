using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField] private List<Renderer> lamps; // 3 Lampen über der Tür, in Reihenfolge
    [SerializeField] private Material lampRedMaterial;
    [SerializeField] private Material lampGreenMaterial;

    [SerializeField] private ServerSpawner serverSpawner;   
    [SerializeField] private EnergyFieldDoor door;          
    [SerializeField] private SelfDefenseSystem defenseSystem; // Runde-3-Fluchtsequenz -- noch zu bauen

    private int currentRound = 1;
    private int serversActiveThisRound = 0;
    private int serversRequiredThisRound = 3;

    public void OnServerActivated()
    {
        serversActiveThisRound++;

        if (serversActiveThisRound >= serversRequiredThisRound)
        {
            if (currentRound < 3)
                TriggerPowerOutage();
            else
                defenseSystem.Trigger();
        }
    }

    private void TriggerPowerOutage()
    {
        door.Lock();
        // Licht flackert, Bugs droppen 
    }

    public void OnSwitchPressed()
    {
        lamps[currentRound - 1].sharedMaterial = lampGreenMaterial;

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