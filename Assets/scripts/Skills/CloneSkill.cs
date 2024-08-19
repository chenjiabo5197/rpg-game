using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone Info")]
    // 实例化的预制件
    [SerializeField] private GameObject clonePrefab;
    // clone体存在的时间
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;

    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        // 根据指定的预制件（Prefab）在场景中创建一个新的实例,如果预制件（Prefab）包含脚本组件，
        // 并且这些脚本组件中有Awake、Start等方法，那么这些方法会在新实例被创建时自动调用
        GameObject newClone = Instantiate(clonePrefab);

        // 设置clone体的坐标，通过获取CloneSkillController脚本中的函数
        // 此处注意由于要实例化CloneSkillController对象，所以CloneSkillController脚本必须在clone对象上，否则一直会空指针
        newClone.GetComponent<CloneSkillController>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset);
    }
}
