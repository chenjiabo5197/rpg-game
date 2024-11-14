using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,   // 护身符
    Flask
}

// 在unity内部新增一个create的新项目，路径是Data/Equipment，创建后默认名为New Item Data，其默认属性有itemName和icon
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
}
