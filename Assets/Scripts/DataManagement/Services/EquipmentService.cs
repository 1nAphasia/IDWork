using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentService
{
    IConfigService _cfg;
    public EquipmentService(IConfigService cfg)
    {
        _cfg = cfg;
    }
    public EquipmentInstance GenInstance(int level, int seed, string templateID, Rarity rarity, int skillId)
    {
        var template = _cfg.GetEquipConfig(templateID);
        var NewIns = new EquipmentInstance(templateID, seed, level, rarity, template, skillId);
        NewIns.grantedSkills.Add(_cfg.GetSkillConfig(skillId));
        return NewIns;
    }
    public static Affix GenerateAffix(System.Random rng, Rarity rarity)
    {
        var statTypes = System.Enum.GetValues(typeof(StatType)).Cast<StatType>().ToArray();

        var statIndex = rng.Next(statTypes.Length);
        var statType = statTypes[statIndex];

        float ratio = (float)rng.NextDouble();

        var (min, max) = AffixRangeTable.Ranges[rarity];
        var (rangeMin, rangeLength) = AffixRangeTable.StatsMinAndMaxbyType[statType];

        float value = rangeMin + ((max - min) * ratio + min) * rangeLength;

        return new Affix
        {
            targetStat = statType,
            value = value,
            type = StatModType.Multiplicative
        };
    }
}