using System.Collections.Generic;
using UnityEngine;


// 弹道数据结构
[System.Serializable]
public class Ballistics
{
    public float spreadAngle = 0f;        // 散布角度
    public float maxDistance = 100f;      // 最大射程
    public float bulletSpeed = 500f;      // 子弹速度(仅对实弹有效)
    public int pelletCount = 1;           // 霰弹枪一次发射的弹丸数量
}

// 武器基类
public class Weapon
{
    public float Damage;
    public float RateOfFire;
    public string templateID;
    public WeaponType? weaponType;
    public int MaxAmmo;
    public ObservableProperty<int> CurrentAmmo;

    public List<FireMode> AvailableFireMode;

    public Weapon(EquipmentInstance EqIns)
    {
        Damage = (float)EqIns.Damage;
        RateOfFire = (float)EqIns.RateofFire;
        templateID = EqIns.templateID;
        weaponType = EqIns.weaponType;
        MaxAmmo = EqIns.MaxAmmo;
        AvailableFireMode = EqIns.AvailableFireMode;
        CurrentAmmo.Value = MaxAmmo;
    }

}

