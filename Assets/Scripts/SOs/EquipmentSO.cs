using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentSO : ScriptableObject
{
    public string templateID;         // weapon_ak47
    public EquipType equipType;
    public EquipSlot equipSlot;
    public string EquipName;
    public Sprite icon;

}

