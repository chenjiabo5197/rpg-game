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
        // ��װ��ж����
        Inventory.instance.UnequipItem(item.data as ItemData_Equipment);
        // ��ж������װ���ŵ�inventory��
        Inventory.instance.AddItem(item.data as ItemData_Equipment);

        ClearUpSlot();
    }
}
