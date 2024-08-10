using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [SerializeField] private GameObject clonePrefab;

    public void CreateClone(Transform _clonePosition)
    {
        // 根据指定的预制件（Prefab）在场景中创建一个新的实例,如果预制件（Prefab）包含脚本组件，
        // 并且这些脚本组件中有Awake、Start等方法，那么这些方法会在新实例被创建时自动调用
        GameObject newClone = Instantiate(clonePrefab);

        // 设置clone体的坐标，通过获取CloneSkillController脚本中的函数
        newClone.GetComponent<CloneSkillController>().SetupClone(_clonePosition);
    }
}
