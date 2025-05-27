
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "TD/Config/Weapon")]
public class WeaponConfigSO : ScriptableObject
{
    public string weaponId;
    public Sprite icon;
    public Rarity rarity;
    public WeaponType weaponType;

    // 基础伤害不再是固定值，而是范围
    public Vector2 baseDamageRange = new Vector2(8, 12); // X = min, Y = max

    // 暴击率范围
    public Vector2 critChanceRange = new Vector2(0.05f, 0.15f);

    public float fireRate;

    // 可能的额外属性池 (更高级)
    public List<PossibleAffixSO> possibleAffixes;
    public int numberOfRandomAffixesToRoll = 2; // 随机生成几个额外词缀
}

// 可选：定义一个 SO 来描述一个可能的词缀及其随机规则
[CreateAssetMenu(menuName = "TD/Config/PossibleAffix")]
public class PossibleAffixSO : ScriptableObject
{
    public StatType targetStat; // 例如：额外伤害、攻击速度、吸血等
    public StatModType modType;
    public Vector2 valueRange;
    public float weight = 1f; // 出现的权重
}