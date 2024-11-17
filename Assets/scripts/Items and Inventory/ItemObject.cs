using UnityEngine;

// 在scene中创建的对象的插件(通过sr定义该对象的渲染图像)
public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    /*
     * 主要用于自定义编辑器中的Inspector面板行为。当用户在Inspector面板中更改了某个字段的值时，如果该字段所属的类中包含OnValidate方法，
     * 那么Unity会自动调用这个方法。这为开发者提供了一个机会来响应字段值的更改，执行一些自定义的验证逻辑，或者更新其他相关的字段
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
        
        // 设置item时更新item的显示，否则item显示会一直是prefab中的图标
        SetupVisuals();
    }

    public void PickupItem()
    {
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
