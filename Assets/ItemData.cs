using UnityEngine;

// ��unity�ڲ�����һ��create������Ŀ��·����Data/Item��������Ĭ����ΪNew Item Data����Ĭ��������itemName��icon
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    // sprite ͼƬ��Դ
    public Sprite icon;
}
