using System;

// �ö���ΪInventory��inventoryItems��inventoryDictionary�е�Ԫ�أ�Ŀ���Ǽ�Inventory�����б���ֵ�Ĺ���
[Serializable]
public class InventoryItem
{
    // ����е���Ʒ
    public ItemData data;
    // �������Ʒ������
    public int stackSize;

    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();
    }

    public void AddStack() => stackSize++;

    public void RemoveStack() => stackSize--;
}
