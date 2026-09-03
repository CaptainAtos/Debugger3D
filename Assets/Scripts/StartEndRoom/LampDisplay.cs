using System.Collections.Generic;
using UnityEngine;

public class LampDisplay : MonoBehaviour
{
    [SerializeField] private List<Renderer> lamps;
    [SerializeField] private Material lampRedMaterial;
    [SerializeField] private Material lampGreenMaterial;

    public void SetRoundComplete(int lampIndex)
    {
        if (lampIndex < 0 || lampIndex >= lamps.Count)
        {
            Debug.LogWarning("LampDisplay: ungültiger Lampen-Index " + lampIndex);
            return;
        }

        lamps[lampIndex].sharedMaterial = lampGreenMaterial;
    }
}
