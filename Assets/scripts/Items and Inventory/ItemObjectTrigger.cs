using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();

    // 该对象的is tirgger是true，在player与该对象发生碰撞后，将该对象加入到Inventory对象的inventoryDictionary中，然后再scene中销毁该对象
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            myItemObject.PickupItem();
        }
    }
}
