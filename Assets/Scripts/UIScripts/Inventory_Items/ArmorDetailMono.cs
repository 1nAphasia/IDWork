using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ArmorDetailMono : MonoBehaviour
{
    public GameObject Panel;
    public TMP_Text ArmorName;
    public TMP_Text Rarity;
    public TMP_Text ArmorType;
    public TMP_Text Armor;
    public TMP_Text CoreStats;
    public TMP_Text Level;
    public Transform coreAffixPanelTransform;
    public GameObject AffixItem;

    public void Setup(EquipmentInstance eq)
    {
        Panel.GetComponent<Image>().sprite = GameDataManager.I.ConfigService.GetRaritySprite(eq.rarity);
        ArmorName.text = eq.template.EquipName;
        Rarity.text = eq.rarity.ToString();
        Utils.RarityToColor.TryGetValue(eq.rarity, out var rarityColor);
        Rarity.color = rarityColor;
        ArmorType.text = eq.weaponType.ToString();
        Armor.text = eq.Damage.ToString();
        CoreStats.text = eq.RateofFire.ToString();
        Level.text = eq.level.ToString();
        foreach (var affix in eq.affixes)
        {
            var affixItem = Instantiate(AffixItem, coreAffixPanelTransform);
            var affixItemMono = affixItem.GetComponent<AffixMono>();
            affixItemMono.Setup(affix, eq.rarity);
        }

    }
}