using System.Collections.Generic;
using UnityEngine;

public static class AffixRangeTable
{
    public static readonly Dictionary<Rarity, (float min, float max)> Ranges = new()
    {
        { Rarity.Common,    (0f, 0.1f) },
        { Rarity.Uncommon,  (0.05f, 0.2f) },
        { Rarity.Rare,      (0.1f, 0.4f) },
        { Rarity.Epic,      (0.2f, 0.8f) },
        { Rarity.Legendary, (0.5f, 1f) },
        { Rarity.Unique,    (0.8f, 1f) }
    };
    public static readonly Dictionary<StatType, (float RangeMin, float RangeLength)> StatsMinAndMaxbyType = new()
    {
    {StatType.MaxHealth,(0,0.1f)},
    {StatType.Armor,(0,0.1f)},
    {StatType.MovementSpeed,(0,0.1f)},
    {StatType.WeaponDamage,(0,0.1f)},
    {StatType.CritChance,(0,0.1f)},
    {StatType.CritDamage,(0,0.1f)},
    {StatType.HeadshotDamage,(0,0.1f)},
    {StatType.WeaponControl,(0,0.1f)},
    {StatType.RateOfFire,(0,0.1f)},
    {StatType.HazardProtection,(0,0.1f)},
    {StatType.DamageReduction,(0,0.1f)},
    {StatType.TotalArmor,(0,0.1f)},
    {StatType.BulletProtection,(0,0.1f)},
    {StatType.SkillDamage,(0,0.1f)},
    {StatType.SkillLength,(0,0.1f)},
    {StatType.CooldownSpeed,(0,0.1f)},
    };
}
public class Utils
{
    public static readonly Dictionary<Rarity, Color> RarityToColor = new()
    {
    {Rarity.Common,Color.white},
    {Rarity.Uncommon,Color.green},
    {Rarity.Rare,Color.blue},
    {Rarity.Legendary,new Color(197,0,225)},
    {Rarity.Epic,Color.yellow},
    };
}


[System.Serializable]
public struct RaritySpritePair
{
    public Rarity rarity;
    public Sprite sprite;
}

[System.Serializable]
public class InventorySaveData
{
    public List<EquipmentInstanceSaveData> items = new();
}

[System.Serializable]
public class EquipmentInstanceSaveData
{
    public string templateID;
    public int seed;
    public int level;
    public Rarity rarity;
}