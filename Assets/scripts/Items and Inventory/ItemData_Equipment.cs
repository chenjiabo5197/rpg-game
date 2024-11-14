using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,   // �����
    Flask
}

// ��unity�ڲ�����һ��create������Ŀ��·����Data/Equipment��������Ĭ����ΪNew Item Data����Ĭ��������itemName��icon
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
}
