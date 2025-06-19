using System.Collections.Generic;
using Unity.Mathematics;

public class EquipmentInstance
{
    public string templateID;
    public int seed;
    public int level;
    public float? Damage;
    public float? RateofFire;
    public WeaponType? weaponType;
    public float? Armor;
    public CoreAffix? CoreAffix;
    public EquipSlot slot;
    public Rarity rarity;
    public EquipmentSO template;
    public List<Affix> affixes;
    public List<SkillSO> grantedSkills;
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
        }
        else
        {
            Armor = math.round(level * math.log2(level) * ((ArmorSO)template).armorValue);
            CoreAffix = ((ArmorSO)template).core;
        }
        var rng = new System.Random(seed);
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
            grantedSkills.Add(new SkillSO() { skillId = (int)skillId });
        }
    }
}