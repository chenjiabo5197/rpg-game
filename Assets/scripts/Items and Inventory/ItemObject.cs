using UnityEngine;

// ��scene�д����Ķ���Ĳ��(ͨ��sr����ö������Ⱦͼ��)
public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    /*
     * ��Ҫ�����Զ���༭���е�Inspector�����Ϊ�����û���Inspector����и�����ĳ���ֶε�ֵʱ��������ֶ����������а���OnValidate������
     * ��ôUnity���Զ����������������Ϊ�������ṩ��һ����������Ӧ�ֶ�ֵ�ĸ��ģ�ִ��һЩ�Զ������֤�߼������߸���������ص��ֶ�
     */
    /*private void OnValidate()
    {
        SetupVisuals();
    }*/

    private void SetupVisuals()
    {
        if (itemData == null)
        {
            return;
        }

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.name;
    }

    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;
        
        // ����itemʱ����item����ʾ������item��ʾ��һֱ��prefab�е�ͼ��
        SetupVisuals();
    }

    public void PickupItem()
    {
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
