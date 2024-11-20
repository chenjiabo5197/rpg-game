using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 在unity内部新增一个create的新项目，路径是Data/Item effect，创建后默认名为New Item Data
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item effect")]
public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffect()
    {
        Debug.Log("Effect execute");
    }
}
