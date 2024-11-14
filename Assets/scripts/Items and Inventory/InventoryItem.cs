using System;

// 该对象为Inventory的inventoryItems和inventoryDictionary中的元素，目的是简化Inventory类中列表和字典的构成
[Serializable]
public class InventoryItem
{
    // 库存中的物品
    public ItemData data;
    // 库存中物品的数量
    public int stackSize;

    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();
    }

    public void AddStack() => stackSize++;

    public void RemoveStack() => stackSize--;
}
