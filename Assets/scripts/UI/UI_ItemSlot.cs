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

    public InventoryItem item;

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        // ʹԤ�Ƽ��ɼ���Ĭ�ϵĦ�Ϊ0��ȫ͸��
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
