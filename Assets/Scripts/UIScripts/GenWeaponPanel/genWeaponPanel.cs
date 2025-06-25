using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;
using System.IO;

public class genWeaponPanel : MonoBehaviour
{
    public TMP_InputField SeedTMP;
    public TMP_InputField LevelTMP;
    public TMP_InputField skillIDTMP;
    public TMP_Dropdown templateID;
    public TMP_Dropdown Rarity;
    public Button GenerateBtn;

    void Start()
    // Start is called once before the first execution of Update after the MonoBehaviour is createdvoid Start()
    {
        InitializePanel();
        // 添加输入合法性校验
        LevelTMP.onEndEdit.AddListener(ValidateLevel);
        skillIDTMP.onEndEdit.AddListener(ValidateSkillID);
        SeedTMP.onEndEdit.AddListener(ValidateSeed);
        GenerateBtn.onClick.AddListener(OnGenerateClicked);
    }

    void InitializePanel()
    {
        var assets = Resources.LoadAll<EquipmentSO>("EquipmentAssets");
        templateID.ClearOptions();
        templateID.AddOptions(assets.Select(a => a.templateID).ToList());

        // 初始化Rarity下拉
        Rarity.ClearOptions();
        Rarity.AddOptions(Enum.GetNames(typeof(Rarity)).ToList());
    }
    void OnEnable()
    {
        InitializePanel();
    }
    void ValidateLevel(string input)
    {
        if (!int.TryParse(input, out int level) || level < 1 || level > 40)
        {
            LevelTMP.text = "";
            Debug.LogWarning("Level必须为1-40的整数");
        }
    }

    void ValidateSkillID(string input)
    {
        if (string.IsNullOrEmpty(input)) return;
        if (!int.TryParse(input, out int skillId) || skillId < 1 || skillId > 1000)
        {
            skillIDTMP.text = "";
            Debug.LogWarning("SkillID必须为空或1-1000的整数");
        }
    }

    void ValidateSeed(string input)
    {
        if (!int.TryParse(input, out _))
        {
            SeedTMP.text = "";
            Debug.LogWarning("Seed必须为整数");
        }
    }

    void OnGenerateClicked()
    {
        if (!int.TryParse(LevelTMP.text, out int level) || level < 1 || level > 40)
        {
            Debug.LogWarning("Level非法");
            return;
        }
        if (!int.TryParse(SeedTMP.text, out int seed))
        {
            Debug.LogWarning("Seed非法");
            return;
        }
        int skillId = 0;
        if (!string.IsNullOrEmpty(skillIDTMP.text))
        {
            if (!int.TryParse(skillIDTMP.text, out skillId) || skillId < 1 || skillId > 1000)
            {
                Debug.LogWarning("SkillID非法");
                return;
            }
        }
        string templateId = templateID.options[templateID.value].text;
        Rarity rarity = (Rarity)Enum.Parse(typeof(Rarity), Rarity.options[Rarity.value].text);

        // 调用生成
        var equipService = GameDataManager.I.EquipService;
        var instance = equipService.GenInstance(level, seed, templateId, rarity, skillId);

        // 加入背包
        GameDataManager.I.InventoryService.AddItem(instance);
        NotificationManager.I.AddNotification(instance.template.name + " has been added to inventory");
    }
}

