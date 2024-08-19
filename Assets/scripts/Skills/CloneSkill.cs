using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone Info")]
    // ʵ������Ԥ�Ƽ�
    [SerializeField] private GameObject clonePrefab;
    // clone����ڵ�ʱ��
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;

    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        // ����ָ����Ԥ�Ƽ���Prefab���ڳ����д���һ���µ�ʵ��,���Ԥ�Ƽ���Prefab�������ű������
        // ������Щ�ű��������Awake��Start�ȷ�������ô��Щ����������ʵ��������ʱ�Զ�����
        GameObject newClone = Instantiate(clonePrefab);

        // ����clone������꣬ͨ����ȡCloneSkillController�ű��еĺ���
        // �˴�ע������Ҫʵ����CloneSkillController��������CloneSkillController�ű�������clone�����ϣ�����һֱ���ָ��
        newClone.GetComponent<CloneSkillController>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset);
    }
}
