using System.IO;
using UnityEditor.Overlays;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager I { get; private set; }
    public IConfigService ConfigService { get; private set; }
    public PlayerStatsService StatsService { get; private set; }
    public InventoryService InventoryService { get; private set; }
    public EquipmentService EquipService { get; private set; }
    public EquipmentSystem EquipSystem { get; private set; }
    public MissionManageService MissionService { get; private set; }
    // …SkillService、RemoteSyncService 等
    string InventorySavePath => Path.Combine("SavedData", "InventorySaveJson.json");
    string SlotSavePath => Path.Combine("SavedData", "SlotSaveJson.json");
    string InventoryJson = null;
    string SlotJson = null;

    private void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        loadAll();

        ConfigService = new ConfigService();
        InventoryService = new InventoryService(ConfigService, StatsService, InventoryJson);
        EquipSystem = new EquipmentSystem(ConfigService, SlotJson);
        StatsService = new PlayerStatsService(EquipSystem);
        EquipService = new EquipmentService(ConfigService);
        MissionService = new MissionManageService();
        // …初始化其他服务
    }

    private void Update()
    {
        StatsService.Update();
        // …其他周期更新
    }
    private void OnDestroy()
    {
        SaveAll();
    }
    public void SaveAll()
    {
        var inventoryData = InventoryService.GetSaveDataJson();
        var playerSlotData = EquipSystem.GetSaveDataJson();

        Savethis(InventorySavePath, inventoryData);
        Savethis(SlotSavePath, playerSlotData);
    }
    public void loadAll()
    {
        if (File.Exists(InventorySavePath))
            InventoryJson = File.ReadAllText(InventorySavePath);
        else
            InventoryJson = null; // 或者给一个默认值

        if (File.Exists(SlotSavePath))
            SlotJson = File.ReadAllText(SlotSavePath);
        else
            SlotJson = null; // 或者给一个默认值
    }
    public void Savethis(string savePath, string data)
    {
        try
        {
            // 创建目录（如果不存在）
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));

            // 保存数据
            File.WriteAllText(savePath, data);
            Debug.Log("保存成功: " + savePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("保存失败: " + e.Message);
        }
    }
}
