using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();

    // �ö����is tirgger��true����player��ö�������ײ�󣬽��ö�����뵽Inventory�����inventoryDictionary�У�Ȼ����scene�����ٸö���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            myItemObject.PickupItem();
        }
    }
}
