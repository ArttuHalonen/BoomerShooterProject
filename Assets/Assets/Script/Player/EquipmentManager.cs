using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [Header("Currently Equipped")]

    [Header("GunStats")]
    public int gunDamage;
    public int gunFireRate;
    public float gunReloadTime;
    public int gunMaxAmmo;
    public int gunCurrentAmmo;

    public bool gunReloading;
}
