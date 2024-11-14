using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UI_ItemSlot : MonoBehaviour
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
}
