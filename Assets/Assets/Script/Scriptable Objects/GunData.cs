using UnityEngine;

[CreateAssetMenu(fileName="Gun", menuName ="Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header("Info")]

    public new string name;

    [Header("Shooting")]

    public float damage;
    public float maxDistance;
    public float fireRate;

    [Header("Reloading")]
    
    public int currentAmmo;
    public int magSize;
    public float reloadTime;
    public bool reloading;











}
