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

    public InventoryItem Item;

    public void UpdateSlot(InventoryItem _newItem)
    {
        Item = _newItem;

        // 使预制件可见，默认的α为0，全透明
        itemImage.color = Color.white;

        if (Item != null)
        {
            itemImage.sprite = Item.data.icon;
            if (Item.stackSize > 1)
            {
                itemText.text = Item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void ClearUpSlot()
    {
        Item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Item.data.itemType == ItemType.Equipment)
        {
            //Debug.Log("Equiped new item + " + Item.data.itemName);
            Inventory.instance.EquipItem(Item.data);
        }
    }
}
