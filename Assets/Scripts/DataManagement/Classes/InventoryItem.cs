// InventoryItem.cs
public class InventoryItem
{
    public EquipmentInstance EquipInst { get; }

    public InventoryItem(EquipmentInstance Inst)
    {
        EquipInst = Inst;
    }
}
