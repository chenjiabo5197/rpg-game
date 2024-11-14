using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UI_ItemSlot : MonoBehaviour
{
    // ��Ⱦ����е������������ͼƬ
    [SerializeField] private Image itemImage;
    // ������Ⱦ����е�����Ʒ����
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem Item;

    public void UpdateSlot(InventoryItem _newItem)
    {
        Item = _newItem;

        // ʹԤ�Ƽ��ɼ���Ĭ�ϵĦ�Ϊ0��ȫ͸��
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
