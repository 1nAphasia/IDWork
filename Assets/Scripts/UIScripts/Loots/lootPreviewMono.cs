using UnityEngine;

public class lootPreviewMono : MonoBehaviour
{
    public TMPro.TMP_Text equipName;
    public void Setup(EquipmentInstance eq)
    {
        equipName.text = eq.template.EquipName;
    }
}