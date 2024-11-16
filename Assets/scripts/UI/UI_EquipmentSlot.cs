using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        // 将装备卸下来
        Inventory.instance.UnequipItem(item.data as ItemData_Equipment);
        // 将卸下来的装备放到inventory中
        Inventory.instance.AddItem(item.data as ItemData_Equipment);

        ClearUpSlot();
    }
}
