using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentSO : ScriptableObject
{
    public string templateID;         // weapon_ak47
    public EquipType equipType;
    public EquipSlot equipSlot;
    public string displayName;
    public Sprite icon;

}

[CreateAssetMenu(menuName = "Equipment/Weapon")]
public class WeaponSO : EquipmentSO
{
    public float baseDamage;
    public float fireRate;
    public WeaponType weaponType;
}
[CreateAssetMenu(menuName = "Equipment/Armor")]
public class ArmorSO : EquipmentSO
{
    public float armorValue;
    public CoreAffix core;
    public ArmorType weaponType;
}