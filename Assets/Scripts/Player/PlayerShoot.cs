using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Animator armsAnimator;
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private AudioSource shootSound;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
            shootSound.PlayOneShot(shootSound.clip);
        }

        bool isAiming = Input.GetMouseButton(1);
        armsAnimator.SetBool("Aim", isAiming);
    }

    void Shoot()
    {
        armsAnimator.SetTrigger("Fire");
        weaponAnimator.SetTrigger("Fire");
        muzzleFlash.Stop();
        muzzleFlash.Play();
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}