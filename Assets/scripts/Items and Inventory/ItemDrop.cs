using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;
    // enemy身上可以掉落的物品列表
    [SerializeField] private ItemData[] possibleDrop;
    // 通过随机值，确认要掉落的物品
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

        // 最多掉落的物品数量，加上此句不会出现数组越界
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
        // 实例化掉落的物品
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        // 掉落物品的初速度设置
        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));
        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
