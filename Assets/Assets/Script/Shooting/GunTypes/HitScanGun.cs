using System.Collections;
using UnityEngine;

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
    [SerializeField] private float ShootDelay = 0.5f;
    private LayerMask Mask;

    private float LastShootTime;

    public void Shoot()
    {
        if (LastShootTime + ShootDelay < Time.time)
        {
            ParticleShootingSystem.Play();
            if (Physics.Raycast(muzzle2.position, muzzle2.forward * 50f, out RaycastHit hit, float.MaxValue, Mask))
            {
                TrailRenderer trail = Instantiate(BulletTrail, muzzle2.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hit));

                LastShootTime = Time.time;
            }
        }
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

   