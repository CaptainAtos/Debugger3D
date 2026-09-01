using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingLampFlicker : MonoBehaviour
{
    [SerializeField] private Light lampLight;
    [SerializeField] private Renderer lampRenderer;
    [SerializeField] private Material litMaterial;
    [SerializeField] private Material unlitMaterial;
    [SerializeField] private float minFlickerInterval = 0.05f;
    [SerializeField] private float maxFlickerInterval = 0.3f;

    private static readonly List<CeilingLampFlicker> activeLamps = new List<CeilingLampFlicker>();

    private Coroutine flickerRoutine;
    private bool isOn = true;

    private void Awake()
    {
        if (lampLight == null)
        {
            lampLight = GetComponent<Light>();
        }

        if (lampRenderer == null)
        {
            lampRenderer = GetComponent<Renderer>();
        }

        if (litMaterial == null && lampRenderer != null)
        {
            litMaterial = lampRenderer.sharedMaterial;
        }
    }

    private void OnEnable()
    {
        activeLamps.Add(this);
    }

    private void OnDisable()
    {
        activeLamps.Remove(this);
    }

    public static void FlickerAll()
    {
        foreach (CeilingLampFlicker lamp in activeLamps)
        {
            lamp.PlayFlicker();
        }
    }

    public static void StopAll()
    {
        foreach (CeilingLampFlicker lamp in activeLamps)
        {
            lamp.StopFlicker();
        }
    }

    private void PlayFlicker()
    {
        if (lampLight == null)
        {
            return;
        }

        if (flickerRoutine != null)
        {
            StopCoroutine(flickerRoutine);
        }
        flickerRoutine = StartCoroutine(FlickerRoutine());
    }

    private void StopFlicker()
    {
        if (flickerRoutine != null)
        {
            StopCoroutine(flickerRoutine);
            flickerRoutine = null;
        }
        SetLampOn(true);
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            SetLampOn(!isOn);
            float wait = Random.Range(minFlickerInterval, maxFlickerInterval);
            yield return new WaitForSeconds(wait);
        }
    }

    private void SetLampOn(bool state)
    {
        isOn = state;

        if (lampLight == null)
        {
            return;
        }

        lampLight.enabled = state;

        if (lampRenderer != null && litMaterial != null && unlitMaterial != null)
        {
            lampRenderer.sharedMaterial = state ? litMaterial : unlitMaterial;
        }
    }
}
