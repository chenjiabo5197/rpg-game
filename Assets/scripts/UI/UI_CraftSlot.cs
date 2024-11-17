using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    /*
     * OnEnable 方法在以下情况下被调用
        首次启用脚本：当脚本首次被添加到GameObject上并启用时。
        重新启用脚本：如果脚本被禁用（通过调用MonoBehaviour.enabled = false），然后再次启用（通过调用MonoBehaviour.enabled = true），
                     OnEnable 方法也会被调用。
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
