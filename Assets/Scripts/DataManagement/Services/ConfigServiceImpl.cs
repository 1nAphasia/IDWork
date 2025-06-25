using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public interface IConfigService
{
    EquipmentSO GetEquipConfig(string equipId);
    BuffConfigSO GetBuffConfig(int buffId);
    SkillSO GetSkillConfig(int skillId);
    public Sprite GetRaritySprite(Rarity rarity);
    // …根据需要扩展
}

public class ConfigService : IConfigService
{
    private Dictionary<string, EquipmentSO> equips;
    private Dictionary<int, BuffConfigSO> buffs;
    private Dictionary<int, SkillSO> skills;
    private Dictionary<Rarity, Sprite> raritySprites;

    public ConfigService()
    {

        equips = Resources
           .LoadAll<EquipmentSO>("EquipmentAssets")
           .ToDictionary(w => w.templateID, w => w);

        buffs = Resources
           .LoadAll<BuffConfigSO>("Buffs")
           .ToDictionary(b => b.buffId, b => b);

        skills = Resources
            .LoadAll<SkillSO>("Skills")
            .ToDictionary(s => s.skillId, s => s);

        raritySprites = new Dictionary<Rarity, Sprite>();
        foreach (Rarity rarity in System.Enum.GetValues(typeof(Rarity)))
        {
            var sprite = Resources.Load<Sprite>($"RarityImage/{rarity}");
            if (sprite != null)
                raritySprites[rarity] = sprite;
        }
    }


    public EquipmentSO GetEquipConfig(string equipId)
        => equips.TryGetValue(equipId, out var w) ? w : null;
    public BuffConfigSO GetBuffConfig(int buffId)
        => buffs.TryGetValue(buffId, out var b) ? b : null;
    public SkillSO GetSkillConfig(int skillId)
    => skills.TryGetValue(skillId, out var s) ? s : null;
    public Sprite GetRaritySprite(Rarity rarity)
    {
        return raritySprites.TryGetValue(rarity, out var sprite) ? sprite : null;
    }
    public IEnumerable<EquipmentSO> GetAllEquipConfigs() => equips.Values;
}
