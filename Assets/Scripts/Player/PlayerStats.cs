// PlayerStats.cs
using System;
using UnityEngine;

public class PlayerStats
{
    public float MaxHealth { get; private set; }
    public float Health { get; private set; }
    public float Armor { get; private set; }
    public float CritChance { get; private set; }
    public float CritDamage { get; private set; }

    public event Action<float> OnHealthChanged;
    public event Action OnStatsDirty;  // 通知 UI/Sync

    public PlayerStats(float maxHealth, float baseArmor, float baseCrit)
    {
        Health = maxHealth;
        Armor = baseArmor;
        CritChance = baseCrit;
    }

    public void TakeDamage(float amount)
    {
        Health = Mathf.Max(0, Health - amount);
        OnHealthChanged?.Invoke(Health);
        OnStatsDirty?.Invoke();
    }

    public void AddBuff(BuffConfigSO buff)
    {
        // 省略：buff 叠加逻辑
        OnStatsDirty?.Invoke();
    }
}
