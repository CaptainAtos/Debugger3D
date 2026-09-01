using UnityEngine;

public class PowerResetSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private RoundManager roundManager;
    [SerializeField] private Transform lever;
    [SerializeField] private float restAngle = 45f;
    [SerializeField] private float activatedAngle = 135f;

    private bool isActivated = false;
    private bool canBeUsed = false;

    public bool IsInteractable => canBeUsed && !isActivated;

    void Start()
    {
        if (roundManager == null)
        {
            roundManager = FindFirstObjectByType<RoundManager>();
        }
        SetLeverAngle(restAngle);
    }

    public bool Interact()
    {
        roundManager.OnSwitchPressed();
        isActivated = true;
        SetLeverAngle(activatedAngle);
        return true;
    }

    public void ResetLever()
    {
        isActivated = false;
        canBeUsed = true;
        SetLeverAngle(restAngle);
    }

    private void SetLeverAngle(float angle)
    {
        Vector3 currentEuler = lever.localEulerAngles;
        lever.localEulerAngles = new Vector3(currentEuler.x, currentEuler.y, angle);
    }
}