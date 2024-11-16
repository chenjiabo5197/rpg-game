using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}

// ��unity�ڲ�����һ��create������Ŀ��·����Data/Item��������Ĭ����ΪNew Item Data����Ĭ��������itemType��itemName��icon
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    // sprite ͼƬ��Դ
    public Sprite icon;
}