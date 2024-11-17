using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;
    // enemy���Ͽ��Ե������Ʒ�б�
    [SerializeField] private ItemData[] possibleDrop;
    // ͨ�����ֵ��ȷ��Ҫ�������Ʒ
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private ItemData item;

    public void GenerateDrop()
    {
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if(Random.Range(0, 100) <= possibleDrop[i].dropChance)
            {
                dropList.Add(possibleDrop[i]);
            }
        }

        // ���������Ʒ���������ϴ˾䲻���������Խ��
        possibleItemDrop = Mathf.Min(possibleItemDrop, dropList.Count);
        for (int i = 0; i < possibleItemDrop; i++)
        {
            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];

            dropList.Remove(randomItem);
            DropItem(randomItem);
        }
    }

    public void DropItem(ItemData _itemData)
    {
        // ʵ�����������Ʒ
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        // ������Ʒ�ĳ��ٶ�����
        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));
        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
