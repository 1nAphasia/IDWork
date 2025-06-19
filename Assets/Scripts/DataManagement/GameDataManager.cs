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
    string InventorySavePath = "Resources/InventorySaveJson.json";
    string SlotSavePath = "Resources/SlotSaveJson.json";
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

        System.IO.File.WriteAllText(InventorySavePath, inventoryData);
        System.IO.File.WriteAllText(SlotSavePath, playerSlotData);
    }
    public void loadAll()
    {
        //InventoryJson = System.IO.File.ReadAllText(InventorySavePath);
        //SlotJson = System.IO.File.ReadAllText(SlotSavePath);
    }
}
