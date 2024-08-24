using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CloneSkill : Skill
{
    [Header("Clone Info")]
    // 实例化的预制件
    [SerializeField] private GameObject clonePrefab;
    // clone体存在的时间
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;

    // 进入和退出dashstate时是否可以创建clone对象
    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;

    // 在counterAttack中反击成功后是否可以创建clone对象
    [SerializeField] private bool canCreateCloneOnCounterAttack;

    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplicateClone;
    // clone对象可被重复创造的比例
    [SerializeField] private float chanceToDuplicate;

    [Header("Crystal instead of clone")]
    // 是否创建crystal对象而非clone对象
    public bool crystalInsteadOfClone;

    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if(crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        // 根据指定的预制件（Prefab）在场景中创建一个新的实例,如果预制件（Prefab）包含脚本组件，
        // 并且这些脚本组件中有Awake、Start等方法，那么这些方法会在新实例被创建时自动调用
        GameObject newClone = Instantiate(clonePrefab);

        // 设置clone体的坐标，通过获取CloneSkillController脚本中的函数
        // 此处注意由于要实例化CloneSkillController对象，所以CloneSkillController脚本必须在clone对象上，否则一直会空指针
        newClone.GetComponent<CloneSkillController>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset, 
            FindClosestEnemy(newClone.transform), canDuplicateClone, chanceToDuplicate);
    }

    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnDashOver()
    {
        if (createCloneOnDashOver)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if(canCreateCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
        }
    }

    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _offset);
    }
}
