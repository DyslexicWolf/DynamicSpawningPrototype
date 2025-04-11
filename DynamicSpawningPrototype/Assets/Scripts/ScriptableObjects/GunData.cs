using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class GunData: ScriptableObject
{
    [Header("Name")]
    public new string name;

    [Header("Shooting")]
    public float damage;
    public float fireRate;
    public float maxDistance;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    public float reloadTime;

    [HideInInspector]
    public bool reloading;
}
