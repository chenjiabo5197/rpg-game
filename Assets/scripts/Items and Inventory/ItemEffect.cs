using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��unity�ڲ�����һ��create������Ŀ��·����Data/Item effect��������Ĭ����ΪNew Item Data
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item effect")]
public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffect()
    {
        Debug.Log("Effect execute");
    }
}
