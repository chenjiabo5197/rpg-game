using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}

// 在unity内部新增一个create的新项目，路径是Data/Item，创建后默认名为New Item Data，其默认属性有itemType、itemName和icon
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    // sprite 图片资源
    public Sprite icon;

    // 物品的掉落几率，0-100之间
    [Range(0, 100)]
    public float dropChance;
}
