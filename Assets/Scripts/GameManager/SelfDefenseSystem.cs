using System.Collections;
using UnityEngine;

public class SelfDefenseSystem : MonoBehaviour
{
    [SerializeField] private float gasCountdownDuration = 60f;

    public static SelfDefenseSystem Instance { get; private set; }

    private Coroutine countdownRoutine;
    private bool hasEscaped = false;

    private void Awake()
    {
        Instance = this;
    }

    public void Trigger()
    {
        hasEscaped = false;

        if (countdownRoutine != null)
        {
            StopCoroutine(countdownRoutine);
        }

        countdownRoutine = StartCoroutine(GasCountdown());
    }

    public void PlayerReachedExit()
    {
        if (hasEscaped)
        {
            return;
        }

        hasEscaped = true;

        if (countdownRoutine != null)
        {
            StopCoroutine(countdownRoutine);
            countdownRoutine = null;
        }

        WinGame();
    }

    private IEnumerator GasCountdown()
    {
        yield return new WaitForSeconds(gasCountdownDuration);

        if (!hasEscaped)
        {
            FillRoomsWithGas();
        }
    }

    private void FillRoomsWithGas()
    {
        // TODO: tödliches Gas in allen Räumen anzeigen + Spieler töten/Game Over auslösen
        Debug.Log("Zeit abgelaufen - die Räume füllen sich mit tödlichem Gas. Game Over.");
    }

    private void WinGame()
    {
        // TODO: Sieg-Screen/Übergang einbauen
        Debug.Log("Rechtzeitig im Start Room angekommen - gewonnen!");
    }
}
