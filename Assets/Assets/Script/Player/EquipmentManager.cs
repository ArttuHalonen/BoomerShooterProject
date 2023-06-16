using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [Header("Currently Equipped")]

    [Header("GunStats")]
    public float gunDamage;
    public float gunFireRate;
    public float gunReloadtime;
    public float gunMaxAmmo;
    public float gunCurrentAmmo;

    public bool gunReloading;
}
