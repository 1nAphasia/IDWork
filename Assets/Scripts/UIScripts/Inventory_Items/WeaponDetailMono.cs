using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponDetailMono : MonoBehaviour
{
    public TMP_Text WeaponName;
    public TMP_Text Rarity;
    public TMP_Text WeaponType;
    public TMP_Text Damage;
    public TMP_Text RPM;
    public TMP_Text MaxAmmo;
    public TMP_Text Level;
    public GameObject RarityBG;
    public Transform coreAffixPanelTransform;
    public GameObject AffixItem;

    public void Setup(EquipmentInstance eq)
    {
        WeaponName.text = eq.template.EquipName;
        Rarity.text = eq.rarity.ToString();
        Utils.RarityToColor.TryGetValue(eq.rarity, out var rarityColor);
        Rarity.color = rarityColor;
        WeaponType.text = eq.weaponType.ToString();
        Damage.text = eq.Damage.ToString();
        RPM.text = eq.RateofFire.ToString();
        MaxAmmo.text = eq.MaxAmmo.ToString();
        Level.text = eq.level.ToString();
        RarityBG.GetComponent<Image>().sprite = GameDataManager.I.ConfigService.GetRaritySprite(eq.rarity);
        foreach (var affix in eq.affixes)
        {
            var affixItem = Instantiate(AffixItem, coreAffixPanelTransform);
            Debug.Log("Instatiated Affix.");
            var affixItemMono = affixItem.GetComponent<AffixMono>();
            affixItemMono.Setup(affix, eq.rarity);
        }

    }
}