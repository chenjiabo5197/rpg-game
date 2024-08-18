using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SwordSkill : Skill
{
    [Header("Skill Info")]
    // 剑的预制体，用于渲染剑扔出去后空中飞行
    [SerializeField] private GameObject swordPrefab;
    // 扔剑的方向
    [SerializeField] private Vector2 launchForce;
    // 扔出去的剑所承受的重力
    [SerializeField] private float swordGravity;
    // 最终扔剑出去的方向
    private Vector2 finalDir;

    [Header("Aim dots")]
    // 瞄准点的数量
    [SerializeField] private int numberOfDots;
    // 每个瞄准点间的距离
    [SerializeField] private float spaceBetweenDots;
    // 瞄准点的预制件
    [SerializeField] private GameObject dotPrefab;
    // 瞄准点的父类，主要用于生成瞄准点时使用
    [SerializeField] private Transform dotsParents;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();

        GenerateDots();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            // normalized是指向量的长度（或称为大小、模）被调整为 1 的向量，同时保持其原始的方向不变
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i< dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    public void CreateSword()
    {
        // 在player的位置实例化一个sword对象
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        // 在实例化对象下获取控制插件
        SwordSkillController newSwordController = newSword.GetComponent<SwordSkillController>();
        // 设置剑的发射方向和重力
        newSwordController.SetupSword(finalDir, swordGravity, player);
        player.AssignNewSword(newSword);
        // 剑已经发射出去了，设置瞄准用的点不可见
        DotsActive(false);
    }

    // 获取瞄准的方向
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        // ScreenToWorldPoint将屏幕上的点（即玩家在屏幕上点击或触摸的点）转换为世界空间中的坐标点
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction;
    }

    // 控制dot的隐藏与展示
    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    // 用dotPrefab初始化dot，初始位置为player所在位置，无旋转
    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            // Quaternion.identity表示没有旋转的四元数
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParents);
            // 默认隐藏dot
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);
        return position;
    }
}
