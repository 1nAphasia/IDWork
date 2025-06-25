using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArmorItem : MonoBehaviour
{
    public TMP_Text ArmorName;
    public TMP_Text ArmorType;
    public TMP_Text Armor;
    public TMP_Text CoreAtrributeNum;
    public Image ArmorIcon;
    public Image Rarity;
    public EquipmentInstance equip;
    public GameObject DetailPanel;
    public GameObject target;
    private GameObject currentSelectedGameobject;

    public void Setup(EquipmentInstance eq, GameObject DetailPanelContainer)
    {
        equip = eq;
        ArmorName.text = eq.template.EquipName;
        ArmorType.text = eq.template.equipType.ToString();
        Armor.text = eq.Armor.ToString();
        CoreAtrributeNum.text = eq.CoreAffix.ToString();

        ArmorIcon.sprite = eq.template.icon;
        Rarity.sprite = GameDataManager.I.ConfigService.GetRaritySprite(eq.rarity);
        target = DetailPanelContainer;
    }
    public void OnClick()
    {

        // 通知列表视图切换任务
        if (EventSystem.current.currentSelectedGameObject == currentSelectedGameobject)
        {
            GameDataManager.I.InventoryService.EquipArmor(equip);
        }
        currentSelectedGameobject = EventSystem.current.currentSelectedGameObject;
    }
    public void OnSelect()
    {
        foreach (Transform child in target.transform)
        {
            Destroy(child.gameObject);
        }
        var Details = Instantiate(DetailPanel, target.transform);
        var dMono = Details.GetComponent<ArmorDetailMono>();
        dMono.Setup(equip);
    }
}