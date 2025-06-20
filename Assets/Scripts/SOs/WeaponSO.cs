using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Equipment/Weapon")]
public class WeaponSO : EquipmentSO
{
    public float baseDamage;
    public float fireRate;
    public WeaponType weaponType;
    public int maxAmmo;
    public Ballistics ballistics;
    public GameObject WeaponPrefab;
    public int minLevel;
    public List<FireMode> AvailableFireMode;
}