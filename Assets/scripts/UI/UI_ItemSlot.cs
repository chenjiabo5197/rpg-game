using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * UI_ItemSlotָ���Ǳ�player���𲢷��뵽�����еĶ�����ֻ�з��봢���еĶ����Ż����UI_ItemSlot�ű��������ڵ����ϵĶ���
 * ��һ���򵥵�Item object����������UI_ItemSlot�ű�������ֻ���ڴ����еĶ����ſ��Դ���OnPointerDown�¼������¼������������/�Ҽ����´���
 */
// IPointerDownHandler�¼�����ָ�밴��ʱ�����ģ����������������������Ҽ�����
public class UI_ItemSlot : MonoBehaviour , IPointerDownHandler
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
