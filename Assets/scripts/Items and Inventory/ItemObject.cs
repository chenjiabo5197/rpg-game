using UnityEngine;

// ��scene�д����Ķ���Ĳ��(ͨ��sr����ö������Ⱦͼ��)
public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    /*
     * ��Ҫ�����Զ���༭���е�Inspector�����Ϊ�����û���Inspector����и�����ĳ���ֶε�ֵʱ��������ֶ����������а���OnValidate������
     * ��ôUnity���Զ����������������Ϊ�������ṩ��һ����������Ӧ�ֶ�ֵ�ĸ��ģ�ִ��һЩ�Զ������֤�߼������߸���������ص��ֶ�
     */
    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.name;
    }

    // �Ķ����is tirgger��true����player��ö�������ײ�󣬽��ö�����뵽Inventory�����inventoryDictionary�У�Ȼ����scene�����ٸö���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
