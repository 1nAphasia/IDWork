
using System;
using System.Collections.Generic;

public class Stats
{
    public PlayerStats playerBaseStats;
    private List<ActiveBuff> buffs = new();
    public Dictionary<StatType, float> finalStats = new();
    private IEquipmentSystem equipSystem;
    // 缓存过的最终属性
    private bool dirty = true;
    public event Action OnStatsChanged;

    public Stats(PlayerStats cfg, IEquipmentSystem eqSystem)
    {
        equipSystem = eqSystem;
        playerBaseStats = cfg;
        MarkDirty();
        Recalculate();
    }

    private void MarkDirty()
    {
        dirty = true;
    }

    public void AddBuff(BuffConfigSO buffCfg)
    {
        buffs.Add(new ActiveBuff(buffCfg));
        MarkDirty();
    }

    // 在 MonoBehaviour 的 Update 中调用
    public void Tick(float deltaTime)
    {
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            if (buffs[i].Tick(deltaTime) <= 0)
            {
                buffs.RemoveAt(i);
                dirty = true;
            }
        }
        if (dirty) Recalculate();
    }

    private void Recalculate()
    {
        //从所有基础属性开始更新当前属性
        // 1. 复制基础属性
        finalStats.Clear();
        foreach (StatType type in Enum.GetValues(typeof(StatType)))
            finalStats[type] = playerBaseStats.GetStat(type);
        if (equipSystem != null)
        {
            foreach (var equip in equipSystem.GetAllEquipped())
            {
                if (equip is null) continue;
                if (equip.template.equipSlot is not EquipSlot.MainHand or EquipSlot.OffHand)
                {
                    finalStats[StatType.Armor] += (float)equip.Armor;
                }
                foreach (var affix in equip.affixes)
                {
                    switch (affix.type)
                    {
                        case StatModType.Additive:
                            finalStats[affix.targetStat] += affix.value;
                            break;
                        case StatModType.Multiplicative:
                            finalStats[affix.targetStat] *= affix.value;
                            break;
                    }
                }

            }
        }
        // 2. 叠加Buff
        foreach (var buff in buffs)
        {
            var t = buff.Config.targetStat;
            var v = buff.Config.value;
            var mod = buff.Config.modType;
            switch (mod)
            {
                case StatModType.Additive:
                    finalStats[t] += v;
                    break;
                case StatModType.Multiplicative:
                    finalStats[t] *= v;
                    break;
            }
        }
        OnStatsChanged?.Invoke();
        dirty = false;
    }
    public void SetWeaponStats(float Damage, float RateofFire, WeaponType weaponType)
    {
        playerBaseStats.baseDamage = Damage;
        playerBaseStats.RateofFire = RateofFire;
        playerBaseStats.weaponType = weaponType;
    }
}

