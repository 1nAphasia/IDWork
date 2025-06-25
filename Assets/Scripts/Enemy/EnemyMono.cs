using UnityEngine;

public class EnemyMono : MonoBehaviour
{
    public EnemyType type;
    public int MaxHealth;
    public int MaxArmor;
    public int Health;
    public int Armor;
    public int level;
    public GameObject healthBarPrefab;
    public GameObject RootCanvas;
    public GameObject lootBeamPrefab;
    void Start()
    {
        var bar = Instantiate(healthBarPrefab, RootCanvas.transform);
        var barScript = bar.GetComponent<EnemyStatusBar>();
        barScript.target = this;
        Health = MaxHealth;
        Armor = MaxArmor;
    }

    public void DamageDelt(int damage)
    {
        if (Armor > 0)
        {
            int armorDamage = Mathf.Min(Armor, damage);
            Armor -= armorDamage;
            damage -= armorDamage;
        }
        if (damage > 0)
        {
            Health -= damage;
        }
        if (Health <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        if (lootBeamPrefab != null)
        {
            var equipService = GameDataManager.I.EquipService;
            // 这里以随机参数为例，你可以根据实际掉落规则调整
            int eqlevel = level; // 敌人等级
            int seed = Random.Range(0, int.MaxValue);
            string templateID = "Rifle_Elephant";
            Rarity rarity;
            if (type is EnemyType.Common) rarity = Rarity.Common;
            else if (type is EnemyType.Rare) rarity = Rarity.Rare;
            else rarity = Rarity.Legendary;

            Vector3 spawnPos = transform.position;
            RaycastHit hit;
            var lootEquip = equipService.GenInstance(eqlevel, seed, templateID, rarity, 0);
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f, LayerMask.GetMask("Default")))
            {
                spawnPos = hit.point;
            }
            var LootBeam = Instantiate(lootBeamPrefab, spawnPos + new Vector3(0, 2, 0), Quaternion.identity);
            var lootMono = LootBeam.GetComponent<LootBeamMono>();
            var mat = LootBeam.GetComponent<MeshRenderer>().material;
            Utils.RarityToColor.TryGetValue(rarity, out var color);
            mat.SetColor("_Color", color);
            if (lootMono != null)
            {
                lootMono.Init(lootEquip);
            }
        }
        Destroy(gameObject);
    }


}