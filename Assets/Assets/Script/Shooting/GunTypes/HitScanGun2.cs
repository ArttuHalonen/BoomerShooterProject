using UnityEngine;
using System.Collections;

public class HitScanGun2 : MonoBehaviour
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

    private float LastShootTime;



    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
    }
    private void Shoot()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        Vector3 direction = GetDirection();
        
        if (Physics.Raycast(muzzle2.position, muzzle2.forward * 50f, direction, out RaycastHit hitInfo, layerMask))
        {
            IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
            damageable.Damage(gunInfo.gunDamage);
            Debug.Log("OW!");
        }
        Debug.DrawRay(muzzle2.position, muzzle2.forward * 50f, Color.red, 0.5f);
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