using System;

// 该对象为Inventory的inventoryItems和inventoryDictionary中的元素，目的是简化Inventory类中列表和字典的构成
[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;

    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();
    }

    public void AddStack() => stackSize++;

    public void RemoveStack() => stackSize--;
}
