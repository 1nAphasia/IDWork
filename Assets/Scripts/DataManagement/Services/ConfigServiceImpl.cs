using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public interface IConfigService
{
    EquipmentSO GetEquipConfig(string equipId);
    BuffConfigSO GetBuffConfig(int buffId);
    SkillSO GetSkillConfig(int skillId);
    // …根据需要扩展
}

public class ConfigService : IConfigService
{
    private Dictionary<string, EquipmentSO> equips;
    private Dictionary<int, BuffConfigSO> buffs;
    private Dictionary<int, SkillSO> skills;

    public ConfigService()
    {

        equips = Resources
           .LoadAll<EquipmentSO>("Config/Equipments")
           .ToDictionary(w => w.templateID, w => w);

        buffs = Resources
           .LoadAll<BuffConfigSO>("Config/Buffs")
           .ToDictionary(b => b.buffId, b => b);

        skills = Resources
            .LoadAll<SkillSO>("Config/Skills")
            .ToDictionary(s => s.skillId, s => s);
    }


    public EquipmentSO GetEquipConfig(string equipId)
        => equips.TryGetValue(equipId, out var w) ? w : null;
    public BuffConfigSO GetBuffConfig(int buffId)
        => buffs.TryGetValue(buffId, out var b) ? b : null;
    public SkillSO GetSkillConfig(int skillId)
    => skills.TryGetValue(skillId, out var s) ? s : null;
    public IEnumerable<EquipmentSO> GetAllEquipConfigs() => equips.Values;
}
