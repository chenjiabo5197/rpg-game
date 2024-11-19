using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * UI_ItemSlot指的是被player捡起并放入到储存中的东西，只有放入储存中的东西才会带有UI_ItemSlot脚本，放置在地面上的东西
 * 是一个简单的Item object，它不包含UI_ItemSlot脚本，所以只有在储存中的东西才可以触发OnPointerDown事件，该事件可以是鼠标左/右键按下触发
 */
// IPointerDownHandler事件是在指针按下时触发的，并不区分是鼠标左键还是右键按下
public class UI_ItemSlot : MonoBehaviour , IPointerDownHandler
{
    // 渲染库存中单个物体的种类图片
    [SerializeField] private Image itemImage;
    // 用于渲染库存中单个物品数量
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem item;

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        // 使预制件可见，默认的α为0，全透明
        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.icon;
            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void ClearUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }
        if (item.data.itemType == ItemType.Equipment)
        {
            //Debug.Log("Equiped new item + " + Item.data.itemName);
            Inventory.instance.EquipItem(item.data);
        }
    }
}
