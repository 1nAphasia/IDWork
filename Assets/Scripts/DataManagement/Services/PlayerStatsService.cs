using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class PlayerStatsService
{
    public Stats playerStats { get; private set; }
    public IEquipmentSystem EquipSystem;
    public event Action OnStatsChanged;

    public PlayerStatsService(IEquipmentSystem EqSystem)
    {
        EquipSystem = EqSystem;
        playerStats = new Stats(new PlayerStats(1), EqSystem);
    }

    public void OnWeaponChange(EquipmentInstance Ins)
    {
        playerStats.SetWeaponStats((float)Ins.Damage, (float)Ins.RateofFire, (WeaponType)Ins.weaponType);
        OnStatsChanged.Invoke();
    }

    public List<(string, string)> GetAllStats()
    {
        List<(string, string)> AllStats = new()
        {
            //Stats Sequence Should be:Level,BaseDamage、RateOfFire、WeaponType,MaxHealth,Armor,MovementSpeed,WeaponDamage,CritChance,CritDamage,HeadshotDamage,WeaponControl,RateOfFire,HazardProtection,DamageReduction,TotalArmor,BulletProtection,SkillDamage,SkillLength,CooldownSpeed
            ("Level",playerStats.playerBaseStats.Level.ToString()),
            ("Health",playerStats.playerBaseStats.Health.ToString()),
            ("BaseDamage",playerStats.playerBaseStats.baseDamage.ToString()),
            ("RateOfFire",playerStats.playerBaseStats.RateofFire.ToString()),
           ("WeaponType",playerStats.playerBaseStats.weaponType.ToString()),
        };
        foreach (var pair in playerStats.finalStats)
        {
            AllStats.Add((pair.Key.ToString(), pair.Value.ToString()));
        }
        return AllStats;
    }

    // 外部调用：每帧更新 Buff 持续时间
    public void Update()
    {
        playerStats.Tick(Time.deltaTime);
    }
}
