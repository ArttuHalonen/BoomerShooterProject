using UnityEngine;

public class ProjectileGun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform muzzle;

    public GameObject projectile;


    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
    }
    private void Shoot()
    {
        Instantiate(projectile, muzzle.transform.position, muzzle.transform.rotation * Quaternion.Euler(0, 0, 0));
    }
}
