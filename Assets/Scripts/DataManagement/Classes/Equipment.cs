#nullable enable
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EquipmentInstance
{
    public string templateID;
    public int seed;
    public int level;
    public float? Damage;
    public float? RateofFire;
    public WeaponType? weaponType;
    public int MaxAmmo;
    public float? Armor;
    public CoreAffix? CoreAffix;
    public Ballistics? ballistics;
    public Rarity rarity;
    public EquipmentSO template;
    public List<Affix> affixes;
    public List<SkillSO> grantedSkills;
    public List<FireMode> AvailableFireMode = new List<FireMode>();
    public EquipmentInstance(string tpID, int sd, int lvl, Rarity ra, EquipmentSO tp, int? skillId)
    {
        templateID = tpID;
        seed = sd;
        level = lvl;
        rarity = ra;
        template = tp;
        if (template.equipSlot is EquipSlot.MainHand or EquipSlot.OffHand)
        {
            Damage = math.round(level * math.log2(level) * ((WeaponSO)template).baseDamage);
            RateofFire = ((WeaponSO)template).fireRate;
            weaponType = ((WeaponSO)template).weaponType;
            MaxAmmo = ((WeaponSO)template).maxAmmo;
            ballistics = ((WeaponSO)template).ballistics;
            AvailableFireMode = ((WeaponSO)template).AvailableFireMode;
        }
        else
        {
            Armor = math.round(level * math.log2(level) * ((ArmorSO)template).armorValue);
            CoreAffix = ((ArmorSO)template).core;
        }
        var rng = new System.Random(seed);
        affixes = new List<Affix>();
        grantedSkills = new List<SkillSO>();


        if (rarity is Rarity.Uncommon or Rarity.Rare)
        {
            affixes.Add(EquipmentService.GenerateAffix(rng, rarity));
        }
        else if (rarity is Rarity.Epic or Rarity.Legendary)
        {
            affixes.Add(EquipmentService.GenerateAffix(rng, rarity));
            affixes.Add(EquipmentService.GenerateAffix(rng, rarity));
        }
        if (skillId is not null)
        {
            var newSO = ScriptableObject.CreateInstance<SkillSO>();
            newSO.skillId = (int)skillId;
            grantedSkills.Add(newSO);
        }
    }
}