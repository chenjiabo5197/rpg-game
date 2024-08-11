using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkill : Skill
{
    [Header("Skill Info")]
    // ����Ԥ���壬������Ⱦ���ӳ�ȥ����з���
    [SerializeField] private GameObject swordPrefab;
    // �ӽ��ķ���
    [SerializeField] private Vector2 launchDir;
    // �ӳ�ȥ�Ľ�������
    [SerializeField] private float swordGravity;

    public void CreateSword()
    {
        // ��player��λ��ʵ����һ��sword����
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        // ��ʵ���������»�ȡ���Ʋ��
        SwordSkillController newSwordController = newSword.GetComponent<SwordSkillController>();
        // ���ý��ķ��䷽�������
        newSwordController.SetupSword(launchDir, swordGravity);
    }
}
