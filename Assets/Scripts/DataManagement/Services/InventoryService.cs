
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryService
{
    IReadOnlyList<InventoryItem> Items { get; }
    event Action<InventoryItem> OnItemAdded;
    event Action<InventoryItem> OnWeaponEquipped;
    void AddItem(EquipmentInstance Ins);
    void EquipWeapon(EquipmentInstance Ins);
}


public class InventoryService : IInventoryService
{
    private List<InventoryItem> _items = new();
    public IReadOnlyList<InventoryItem> Items => _items;
    public event Action<InventoryItem> OnItemAdded;
    public event Action<InventoryItem> OnWeaponEquipped;
    private PlayerStatsService _stats;
    private IConfigService _cfg;

    public InventoryService(IConfigService cfg, PlayerStatsService stats, string json)
    {
        _stats = stats;
        _cfg = cfg;
        if (json is not null)
        {
            LoadFromSaveData(json);
            Debug.Log("Inventory Successfully Load.");
        }
        Debug.Log("Inventory Fail to Load.");
    }

    public void AddItem(EquipmentInstance Ins)
    {
        var item = new InventoryItem(Ins);
        _items.Add(item);
        OnItemAdded?.Invoke(item);
    }

    public void AddItemToInventory(EquipmentInstance Ins)
    {
        var item = new InventoryItem(Ins);
        _items.Add(item);
        NotificationManager.I.AddNotification(Ins.template.EquipName + " has been added to Inventory.");
        OnItemAdded?.Invoke(item);
    }

    public void EquipWeapon(EquipmentInstance Ins)
    {
        NotificationManager.I.AddNotification(Ins.template.EquipName + " equipped.");
        GameDataManager.I.EquipSystem.Equip(Ins);
    }
    public void EquipArmor(EquipmentInstance Ins)
    {
        NotificationManager.I.AddNotification(Ins.template.EquipName + " equipped.");
        GameDataManager.I.EquipSystem.Equip(Ins);
    }
    public void LoadFromSaveData(string json)
    {
        InventorySaveData saveData = JsonUtility.FromJson<InventorySaveData>(json);

        foreach (var equipData in saveData.items)
        {
            var template = _cfg.GetEquipConfig(equipData.templateID);
            var equipInst = new EquipmentInstance
            (
                equipData.templateID,
                equipData.seed,
                equipData.level,
                equipData.rarity,
                template,
                null
            );
            AddItem(equipInst);
        }
    }
    public string GetSaveDataJson()
    {
        InventorySaveData saveData = new InventorySaveData();
        foreach (var item in Items)
        {
            var equip = item.EquipInst;
            saveData.items.Add(new EquipmentInstanceSaveData
            {
                templateID = equip.templateID,
                seed = equip.seed,
                level = equip.level,
                rarity = equip.rarity
                // 其它字段
            });
        }
        string json = JsonUtility.ToJson(saveData);
        return json;
    }
}
