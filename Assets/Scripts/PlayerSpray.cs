using UnityEngine;

public class PlayerSpray : MonoBehaviour
{
    [SerializeField] private ParticleSystem sprayEffect;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float range = 5f;
    [SerializeField] private float sprayAngle = 30f;
    [SerializeField] private float damageInterval = 0.5f;

    private float damageTimer = 0f;
    private bool isSpraying = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            StartSpraying();

        if (Input.GetMouseButtonUp(0))
            StopSpraying();

        if (isSpraying)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                DealDamage();
                damageTimer = 0f;
            }
        }
    }

    void StartSpraying()
    {
        isSpraying = true;
        damageTimer = 0f;

        if (sprayEffect != null)
            sprayEffect.Play();
    }

    void StopSpraying()
    {
        isSpraying = false;

        if (sprayEffect != null)
            sprayEffect.Stop();
    }

    void DealDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);

        foreach (Collider hitCollider in hitColliders)
        {
            Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget <= sprayAngle)
            {
                IDamageable damageable = hitCollider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damage);
                }
            }
        }
    }
}