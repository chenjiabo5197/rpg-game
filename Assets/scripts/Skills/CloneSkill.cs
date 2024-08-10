using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [SerializeField] private GameObject clonePrefab;

    public void CreateClone(Transform _clonePosition)
    {
        // ����ָ����Ԥ�Ƽ���Prefab���ڳ����д���һ���µ�ʵ��,���Ԥ�Ƽ���Prefab�������ű������
        // ������Щ�ű��������Awake��Start�ȷ�������ô��Щ����������ʵ��������ʱ�Զ�����
        GameObject newClone = Instantiate(clonePrefab);

        // ����clone������꣬ͨ����ȡCloneSkillController�ű��еĺ���
        newClone.GetComponent<CloneSkillController>().SetupClone(_clonePosition);
    }
}
