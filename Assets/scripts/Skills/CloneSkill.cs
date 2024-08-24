using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CloneSkill : Skill
{
    [Header("Clone Info")]
    // ʵ������Ԥ�Ƽ�
    [SerializeField] private GameObject clonePrefab;
    // clone����ڵ�ʱ��
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;

    // ������˳�dashstateʱ�Ƿ���Դ���clone����
    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;

    // ��counterAttack�з����ɹ����Ƿ���Դ���clone����
    [SerializeField] private bool canCreateCloneOnCounterAttack;

    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplicateClone;
    // clone����ɱ��ظ�����ı���
    [SerializeField] private float chanceToDuplicate;

    [Header("Crystal instead of clone")]
    // �Ƿ񴴽�crystal�������clone����
    public bool crystalInsteadOfClone;

    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if(crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        // ����ָ����Ԥ�Ƽ���Prefab���ڳ����д���һ���µ�ʵ��,���Ԥ�Ƽ���Prefab�������ű������
        // ������Щ�ű��������Awake��Start�ȷ�������ô��Щ����������ʵ��������ʱ�Զ�����
        GameObject newClone = Instantiate(clonePrefab);

        // ����clone������꣬ͨ����ȡCloneSkillController�ű��еĺ���
        // �˴�ע������Ҫʵ����CloneSkillController��������CloneSkillController�ű�������clone�����ϣ�����һֱ���ָ��
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
