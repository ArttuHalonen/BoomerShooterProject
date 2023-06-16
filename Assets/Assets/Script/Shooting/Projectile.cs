using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] private EquipmentManager gunInfo;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 50f, ForceMode.Impulse);
        Destroy(this.gameObject, 2f);
    }
    private void OnTriggerEnter (Collider other)
    {
        if (TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.Damage(gunInfo.gunDamage);
            Debug.Log("OW");
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
}
