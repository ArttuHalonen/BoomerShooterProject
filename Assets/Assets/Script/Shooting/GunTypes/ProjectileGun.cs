using System.Collections;
using UnityEngine;

public class ProjectileGun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform muzzle;
    [SerializeField] private float ShootDelay;
    [SerializeField] private EquipmentManager gunInfo;

    public GameObject projectile;


    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
    }
    public void StartReload()
    {
        if (!gunInfo.gunReloading)
        {
            StartCoroutine(Reload());
        }
    }
    private IEnumerator Reload()
    {
        gunInfo.gunReloading = true;

        yield return new WaitForSeconds(gunInfo.gunReloadTime);

        gunInfo.gunCurrentAmmo = gunInfo.gunMaxAmmo;

        gunInfo.gunReloading = false;
    }
    private bool CanShoot() => !gunInfo.gunReloading && ShootDelay > 1f / (gunInfo.gunFireRate / 60f);
    private void Shoot()
    {
        //check if mag is empty
        if (gunInfo.gunCurrentAmmo > 0)
        {
            //check if CanShoot is True
            if (CanShoot())
            {
                //spawn the bullit
                Instantiate(projectile, muzzle.transform.position, muzzle.transform.rotation * Quaternion.Euler(0, 0, 0));
            }
            //update gunInfo
            gunInfo.gunCurrentAmmo--;
            ShootDelay = 0;
        }
    }
    private void Update()
    {
        ShootDelay += Time.deltaTime;
    }
}
