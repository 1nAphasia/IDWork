using UnityEngine;

public class LootDetailMono : MonoBehaviour
{
    public GameObject Content;
    public GameObject DetailContainer;
    public GameObject WeaponItemPrefab;
    public GameObject lootBeam;
    public void Setup(EquipmentInstance eq, GameObject lb)
    {
        foreach (Transform child in Content.transform)
        {
            Destroy(child.gameObject);
        }
        lootBeam = lb;
        var WeaponItem = Instantiate(WeaponItemPrefab, Content.transform);
        WeaponItem.GetComponent<WeaponItem>().Setup(eq, DetailContainer);
    }
    void Update()
    {
        if (Content.transform.childCount == 0)
            Destroy(lootBeam);
    }
}