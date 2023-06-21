using UnityEngine;
using System.Collections;
using UnityEngine.ProBuilder.MeshOperations;

public class HitScanGun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform muzzle2;
    [SerializeField] private EquipmentManager gunInfo;

    [Header("Behaviour")]
    [SerializeField] private bool AddBulletSpread = true;
    [SerializeField] private Vector3 BulletSpreadVariance = new(0.1f, 0.1f, 0.1f);
    [SerializeField] private ParticleSystem ParticleShootingSystem;
    [SerializeField] private ParticleSystem ImpactParticleSystem;
    [SerializeField] TrailRenderer BulletTrail;
    [SerializeField] private float ShootDelay;
    [SerializeField] LayerMask rayMask;

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
            //check if youre reloading and how much time passed since last shot
            if (CanShoot())
            {
                ParticleShootingSystem.Play();
                if (Physics.Raycast(muzzle2.position, muzzle2.forward * 50f, out RaycastHit hitInfo, rayMask))
                {
                    //spawn trails indicated in SpawnTrail subroutine
                    TrailRenderer trail = Instantiate(BulletTrail, muzzle2.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail, hitInfo));
                    //Debug.DrawLine(muzzle2.position, hitInfo.point, Color.red, 0.5f);
                    {
                        //find out if you hit a target capable of taking damage
                        if (hitInfo.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
                        {
                            //deal damage = guns damage in equipment manager
                            damageable.Damage(gunInfo.gunDamage);
                            Debug.Log(hitInfo.collider.gameObject);
                        }
                        //update gunInfo
                        gunInfo.gunCurrentAmmo--;
                        ShootDelay = 0;
                    }
                }
            }
        }
    }
    private void Update()
    {
        ShootDelay += Time.deltaTime;
    }
    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;

        if (AddBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
                Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
                Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)
            );

            direction.Normalize();
        }

        return direction;
    }


    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit Hit)
    {
        float time = 0;
        Vector3 startPosition = Trail.transform.position;

        while (time < 1)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, Hit.point, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }
        Trail.transform.position = Hit.point;
        Instantiate(ImpactParticleSystem, Hit.point, Quaternion.LookRotation(Hit.normal));

        Destroy(Trail.gameObject, Trail.time);
    }
}