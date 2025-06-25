using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponItem : MonoBehaviour
{
    public TMP_Text WeaponName;
    public TMP_Text WeaponType;
    public TMP_Text Damage;
    public TMP_Text DPM;
    public Image WeaponImage;
    public Image RarityBG;
    public EquipmentInstance equip;
    public GameObject DetailPanel;
    public GameObject target;
    private GameObject currentSelectedGameobject;

    public void Setup(EquipmentInstance eq, GameObject DetailPanelContainer)
    {
        equip = eq;
        WeaponName.text = eq.template.EquipName;
        WeaponType.text = eq.weaponType.ToString();
        Damage.text = eq.Damage.ToString();
        DPM.text = eq.RateofFire.ToString();
        WeaponImage.sprite = eq.template.icon;
        RarityBG.sprite = GameDataManager.I.ConfigService.GetRaritySprite(eq.rarity);

        target = DetailPanelContainer;
    }
    public void OnClick()
    {
        // 通知列表视图切换任务
        if (EventSystem.current.currentSelectedGameObject == currentSelectedGameobject)
        {
            GameDataManager.I.InventoryService.EquipWeapon(equip);
        }
        currentSelectedGameobject = EventSystem.current.currentSelectedGameObject;
    }
    public void LootOnClick()
    {
        if (EventSystem.current.currentSelectedGameObject == currentSelectedGameobject)
        {
            GameDataManager.I.InventoryService.AddItemToInventory(equip);
            UIManager.Instance.CloseLootDetailPanel();
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
        var dMono = Details.GetComponent<WeaponDetailMono>();
        dMono.Setup(equip);
    }
}