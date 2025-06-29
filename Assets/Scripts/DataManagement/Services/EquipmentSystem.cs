using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public interface IEquipmentSystem
{
    IEnumerable<EquipmentInstance> GetAllEquipped();
    EquipmentInstance GetEquipped(EquipSlot slot);
    void Equip(EquipmentInstance equip);
    void Unequip(EquipSlot slot);
}
public class EquipmentSystem : IEquipmentSystem
{
    private Dictionary<EquipSlot, EquipmentInstance> equipped = new();
    public IConfigService _cfg;
    public event Action EquipChanged;
    public EquipmentSystem(IConfigService cfg, string json)
    {
        _cfg = cfg;
        if (json is null)
        {
            foreach (EquipSlot slot in Enum.GetValues(typeof(EquipSlot)))
            {
                equipped[slot] = null;
            }
        }
        else
        {
            LoadFromSaveData(json, _cfg);
        }
    }
    public IEnumerable<EquipmentInstance> GetAllEquipped()
    {
        return equipped.Values;
    }
    public EquipmentInstance GetEquipped(EquipSlot slot)
    {
        equipped.TryGetValue(slot, out var equip);
        return equip;
    }
    public void Equip(EquipmentInstance equip)
    {
        if (equip == null) return;
        equipped[equip.template.equipSlot] = equip;
        EquipChanged.Invoke();
    }

    public void Unequip(EquipSlot slot)
    {
        equipped.Remove(slot);
    }

    public string GetSaveDataJson()
    {
        var saveData = new PlayerEquipmentSaveData();
        foreach (var kv in equipped)
        {
            var equip = kv.Value;
            if (equip != null)
            {
                var newdata = new EquipmentSaveData
                {
                    slot = kv.Key,
                    templateID = equip.templateID,
                    level = equip.level,
                    seed = equip.seed,
                    rarity = equip.rarity,
                };
                if (equip.grantedSkills.Count is not 0)
                    newdata.skillId = equip.grantedSkills[0].skillId;
                saveData.equipped.Add(newdata);
            }
        }
        string json = JsonUtility.ToJson(saveData);
        Debug.Log("slot save json is: " + json);

        return json;
    }
    public void LoadFromSaveData(string json, IConfigService cfg)
    {
        var saveData = JsonUtility.FromJson<PlayerEquipmentSaveData>(json);
        equipped.Clear();
        foreach (var slot in Enum.GetValues(typeof(EquipSlot)))
        {
            equipped[(EquipSlot)slot] = null;
        }
        foreach (var data in saveData.equipped)
        {
            var template = cfg.GetEquipConfig(data.templateID);
            var equip = new EquipmentInstance
            (
                data.templateID,
                data.level,
                data.seed,
                data.rarity,
                template,
                data.skillId
            );
            equipped[data.slot] = equip;
        }
    }
}
[System.Serializable]
public class EquipmentSaveData
{
    public EquipSlot slot;
    public string templateID;
    public int level;
    public int seed;
    public Rarity rarity;
    public int skillId;

}

[System.Serializable]
public class PlayerEquipmentSaveData
{
    public List<EquipmentSaveData> equipped = new();
}

