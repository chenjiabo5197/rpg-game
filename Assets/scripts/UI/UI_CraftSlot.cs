using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    /*
     * OnEnable ��������������±�����
        �״����ýű������ű��״α���ӵ�GameObject�ϲ�����ʱ��
        �������ýű�������ű������ã�ͨ������MonoBehaviour.enabled = false����Ȼ���ٴ����ã�ͨ������MonoBehaviour.enabled = true����
                     OnEnable ����Ҳ�ᱻ���á�
     */
    private void OnEnable()
    {
        UpdateSlot(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Equipment craftData = item.data as ItemData_Equipment;

        Inventory.instance.CanCraft(craftData, craftData.craftingMaterials);
    }
}
