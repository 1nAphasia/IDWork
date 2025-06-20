
using System;
using System.Collections.Generic;

public class PlayerStats
{
    public PlayerBaseStats playerBaseStats;
    private List<ActiveBuff> buffs = new();
    public Dictionary<StatType, float> finalStats = new();
    private IEquipmentSystem equipSystem;
    private bool dirty = true;
    public float Damage;
    public float RateofFire = 0;
    public WeaponType CurrentWeaponType;
    public event Action OnStatsChanged;
    public List<Weapon> Weapons;
    public int CurrentWeapon = 0;

    public PlayerStats(PlayerBaseStats cfg, IEquipmentSystem eqSystem)
    {
        equipSystem = eqSystem;
        playerBaseStats = cfg;
        Damage = playerBaseStats.baseDamage;
        InitiateWeapons();
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
        //2. 计算装备属性
        if (equipSystem != null)
        {
            foreach (var equip in equipSystem.GetAllEquipped())
            {
                if (equip is null) continue;
                //叠甲
                if (equip.template.equipSlot is not EquipSlot.MainHand or EquipSlot.OffHand)
                {
                    finalStats[StatType.Armor] += (float)equip.Armor;
                }
                Damage = Weapons[CurrentWeapon].Damage + playerBaseStats.baseDamage;
                RateofFire = Weapons[CurrentWeapon].RateOfFire;
                CurrentWeaponType = (WeaponType)Weapons[CurrentWeapon].weaponType;
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

    private void InitiateWeapons()
    {
        foreach (var equip in equipSystem.GetAllEquipped())
        {
            if (equip is null || equip.template.equipSlot is not EquipSlot.MainHand or EquipSlot.OffHand) continue;
            //叠甲
            if (equip.template.equipSlot is EquipSlot.MainHand)
            {
                Weapons.Add(new Weapon(equip));
            }
            if (equip.template.equipSlot is EquipSlot.OffHand)
            {
                Weapons.Add(new Weapon(equip));
            }
        }
    }

    public void SetWeaponStats(float Damage, float RateofFire, int CurrentAmmo, int MaxAmmo, WeaponType weaponType)
    {

    }
}

