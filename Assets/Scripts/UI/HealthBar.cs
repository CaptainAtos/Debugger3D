using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Slider healthSlider;

    void Update()
    {
        healthSlider.value = playerHealth.CurrentHealth / playerHealth.MaxHealth;
    }
}
