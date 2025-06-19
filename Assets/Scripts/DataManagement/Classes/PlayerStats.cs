// PlayerStats.cs
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    [Range(1, 40)]
    public uint Level { get; private set; }
    public float Health = 1.0f;
    public float baseDamage = 0.0f;
    public float RateofFire = 0.0f;
    public WeaponType weaponType = WeaponType.Pistol;

    private Dictionary<StatType, float> stats = new();

    public float GetStat(StatType type) => stats.TryGetValue(type, out var v) ? v : 0f;
    public void SetStat(StatType type, float value) => stats[type] = value;
    public PlayerStats(uint level)
    {
        Level = level;
        // 初始化基础属性
        stats[StatType.MaxHealth] = 1000 + Level * Mathf.Log(Level) * 40;
        Health = stats[StatType.MaxHealth];
        stats[StatType.Armor] = 2500 + Level * Mathf.Log(Level) * 100;
        stats[StatType.MovementSpeed] = 20;
        stats[StatType.WeaponDamage] = 0;
        stats[StatType.CritChance] = 0;
        stats[StatType.CritDamage] = 1.5f;
        stats[StatType.HeadshotDamage] = 1.5f;
        stats[StatType.WeaponControl] = 0;
        stats[StatType.RateOfFire] = 0;
        stats[StatType.HazardProtection] = 0;
        stats[StatType.DamageReduction] = 0;
        stats[StatType.TotalArmor] = 0;
        stats[StatType.BulletProtection] = 0;
        stats[StatType.SkillDamage] = 0;
        stats[StatType.SkillLength] = 0;
        stats[StatType.CooldownSpeed] = 0;
    }
    public void TakeDamage(float amount)
    {
        Health = Mathf.Max(0, Health - amount);
    }

    public void Heal(float amount)
    {
        Health = Math.Min(stats[StatType.MaxHealth], Health + amount);
    }
    public void LevelUp()
    {
        Level++;
    }
}
