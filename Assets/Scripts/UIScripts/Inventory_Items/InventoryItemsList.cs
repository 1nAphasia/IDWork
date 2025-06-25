using UnityEngine;
using UnityEngine.UI;

public class InventoryItemsList : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject WeaponItem;
    public GameObject ArmorItem;
    public GameObject Content;
    public GameObject DetailPanelContainer;
    public EquipSlot currentSlot;


    void Start()
    {

    }
    public void Setup(EquipSlot slot)
    {
        currentSlot = slot;
        foreach (Transform child in Content.transform)
        {
            Destroy(child.gameObject);
        }
        if (currentSlot is EquipSlot.MainHand or EquipSlot.OffHand)
        {
            SetupWeaponList();
        }
        else
        {
            SetupArmorList();
        }
    }

    void SetupWeaponList()
    {
        var ItemList = GameDataManager.I.InventoryService.Items;
        foreach (var item in ItemList)
        {
            if (item.EquipInst.template.equipSlot == currentSlot)
            {
                GameObject NewItem = Instantiate(WeaponItem, Content.transform);
                var ItemMono = NewItem.GetComponent<WeaponItem>();
                ItemMono.Setup(item.EquipInst, DetailPanelContainer);
            }
        }
    }
    void SetupArmorList()
    {
        var ItemList = GameDataManager.I.InventoryService.Items;
        foreach (var item in ItemList)
        {
            if (item.EquipInst.template.equipSlot == currentSlot)
            {
                GameObject NewItem = Instantiate(ArmorItem, Content.transform);
                var ItemMono = NewItem.GetComponent<ArmorItem>();
                ItemMono.Setup(item.EquipInst, DetailPanelContainer);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
