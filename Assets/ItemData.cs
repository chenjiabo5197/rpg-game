using UnityEngine;

// 在unity内部新增一个create的新项目，路径是Data/Item，创建后默认名为New Item Data，其默认属性有itemName和icon
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    // sprite 图片资源
    public Sprite icon;
}
