using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Animator armsAnimator;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.5f;

    private float fireTimer = 0f;

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }
    }

    void Shoot()
    {
        armsAnimator.SetTrigger("Fire");
        muzzleFlash.Play();
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
