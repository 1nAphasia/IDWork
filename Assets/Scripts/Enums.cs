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
    Health,
    MaxHealth,
    Armor,
    Shield,

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
    Additive,
    Multiplicative,
    PercentAdd,
    PercentMult,

    FinalAdditive,
    FinalMultiplicative,
    FinalPercentAdd,
    FinalPercentMult,
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