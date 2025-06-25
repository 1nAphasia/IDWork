using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotButtonMono : MonoBehaviour
{
    public EquipSlot slotType;
    public List<GameObject> ModifiedStats;
    public void Setup()
    {
        var eq = GameDataManager.I.EquipSystem.GetEquipped(slotType);
        if (eq is null) return;
        if (slotType is EquipSlot.MainHand or EquipSlot.OffHand)
        {
            ModifiedStats[0].GetComponent<Image>().sprite = eq.template.icon;
            ModifiedStats[1].GetComponent<TMP_Text>().text = eq.template.EquipName;
            ModifiedStats[2].GetComponent<TMP_Text>().text = ((int)(eq.Damage * eq.RateofFire / 60)).ToString();
            ModifiedStats[3].GetComponent<Image>().sprite = GameDataManager.I.ConfigService.GetRaritySprite(eq.rarity);
        }
        else
        {
            Debug.Log("111");
            ModifiedStats[0].GetComponent<Image>().sprite = eq.template.icon;
            ModifiedStats[1].GetComponent<Image>().sprite = GameDataManager.I.ConfigService.GetRaritySprite(eq.rarity);
        }
    }
}