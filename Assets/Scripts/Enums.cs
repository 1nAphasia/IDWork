using UnityEngine;

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Unique
}

public enum StatType
{
    MaxHealth,
    Armor,
    MovementSpeed,
    WeaponDamage,
    CritChance,
    CritDamage,
    HeadshotDamage,
    WeaponControl,
    RateOfFire,
    HazardProtection,
    DamageReduction,
    TotalArmor,
    BulletProtection,
    SkillDamage,
    SkillLength,
    CooldownSpeed,
}

public enum StatModType
{
    Flat,//白值加算
    Additive,
    Multiplicative,

    FinalAdditive,
    FinalMultiplicative,

}

public enum EquipType
{
    Weapon,
    Armor,
}

public enum WeaponType
{
    Rifle,
    Shotgun,
    Pistol,
    Sniper,
    MicroMachinegun,
    LightMachinegun,

}

public enum ArmorType
{
    Chest,
    Helmet,
    Legs,
    Gloves,
    Backpack,
}
public enum EquipSlot
{
    MainHand,
    OffHand,
    Chest,
    Helmet,
    Legs,
    Gloves,
    Backpack,
}

public enum CoreAffix
{
    Attack,
    Defense,
    Skill
}

public enum MissionStatus
{
    InActive,
    Active,
    Completed,
}

public enum FireMode { SemiAuto, FullAuto, Burst, Charge, SingleShot }

public enum EnemyType { Common, Rare, Elite }