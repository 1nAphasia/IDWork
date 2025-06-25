using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsService
{
    public PlayerStats playerStats { get; private set; }
    public IEquipmentSystem EquipSystem;
    public event Action OnStatsChanged;
    public PlayerStatsService(IEquipmentSystem EqSystem)
    {
        EquipSystem = EqSystem;
        playerStats = new PlayerStats(new PlayerBaseStats(1), EqSystem);
    }


    public List<(string, string)> GetAllStats()
    {
        List<(string, string)> AllStats = new()
        {
            //Stats Sequence Should be:Level,BaseDamage、RateOfFire、WeaponType,MaxHealth,Armor,MovementSpeed,WeaponDamage,CritChance,CritDamage,HeadshotDamage,WeaponControl,RateOfFire,HazardProtection,DamageReduction,TotalArmor,BulletProtection,SkillDamage,SkillLength,CooldownSpeed
            ("Level",playerStats.playerBaseStats.Level.ToString()),
            ("Health",playerStats.playerBaseStats.Health.ToString()),
            ("Damage",playerStats.Damage.ToString()),
            ("RateOfFire",playerStats.RateofFire.ToString()),
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
