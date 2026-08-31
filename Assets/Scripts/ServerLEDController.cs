using System.Collections;
using UnityEngine;

public class ServerLEDController : MonoBehaviour, IInteractable
{
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material greenMaterial;

    [SerializeField] private float minFlickerInterval = 0.05f;
    [SerializeField] private float maxFlickerInterval = 0.4f;

    private Renderer[] leds;
    private bool isActive = false;

    public bool IsInteractable => !isActive;

    public event System.Action OnActivated;

    private void Awake()
    {
        leds = GetComponentsInChildren<Renderer>();
        SetAllRed();
    }

    public void SetActive(bool active)
    {
        isActive = active;

        if (!isActive)
        {
            StopAllCoroutines();
            SetAllRed();
            return;
        }

        foreach (Renderer led in leds)
        {
            led.sharedMaterial = greenMaterial;
            StartCoroutine(FlickerLoop(led));
        }
    }

    private void SetAllRed()
    {
        foreach (Renderer led in leds)
        {
            led.enabled = true;
            led.sharedMaterial = redMaterial;
        }
    }

    private IEnumerator FlickerLoop(Renderer led)
    {
        while (isActive)
        {
            led.enabled = !led.enabled;
            float wait = Random.Range(minFlickerInterval, maxFlickerInterval);
            yield return new WaitForSeconds(wait);
        }
        led.enabled = true;
    }

    public bool Interact()
    {
        SetActive(!isActive);
        if (isActive)
            OnActivated?.Invoke();
        return true;
    }
}