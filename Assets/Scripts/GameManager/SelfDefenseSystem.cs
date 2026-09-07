using System.Collections;
using UnityEngine;

public class SelfDefenseSystem : MonoBehaviour
{
    [SerializeField] private float gasCountdownDuration = 60f;
    [SerializeField] private PlayerHealth playerHealth;

    public static SelfDefenseSystem Instance { get; private set; }

    private Coroutine countdownRoutine;
    private bool hasEscaped = false;

    private void Awake()
    {
        Instance = this;

        if (playerHealth == null)
            playerHealth = FindFirstObjectByType<PlayerHealth>();
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
        Debug.Log("Zeit abgelaufen - die Räume füllen sich mit tödlichem Gas. Game Over.");

        if (playerHealth != null)
            playerHealth.Die();
    }

    private void WinGame()
    {
        Debug.Log("Rechtzeitig im Start Room angekommen - gewonnen!");
    }
}
