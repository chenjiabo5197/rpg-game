using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkill : Skill
{
    [Header("Skill Info")]
    // 剑的预制体，用于渲染剑扔出去后空中飞行
    [SerializeField] private GameObject swordPrefab;
    // 扔剑的方向
    [SerializeField] private Vector2 launchDir;
    // 扔出去的剑的重力
    [SerializeField] private float swordGravity;

    public void CreateSword()
    {
        // 在player的位置实例化一个sword对象
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        // 在实例化对象下获取控制插件
        SwordSkillController newSwordController = newSword.GetComponent<SwordSkillController>();
        // 设置剑的发射方向和重力
        newSwordController.SetupSword(launchDir, swordGravity);
    }
}
