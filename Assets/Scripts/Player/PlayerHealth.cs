using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable, IKillable
{
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;
    private bool isDead = false;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    public event System.Action OnDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;
        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        isDead = true;

        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
            movement.enabled = false;

        Debug.Log("Player ist gestorben - Game Over");
        OnDeath?.Invoke();
    }
}
