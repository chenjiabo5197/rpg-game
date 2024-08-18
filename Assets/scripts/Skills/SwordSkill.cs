using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SwordSkill : Skill
{
    [Header("Skill Info")]
    // ����Ԥ���壬������Ⱦ���ӳ�ȥ����з���
    [SerializeField] private GameObject swordPrefab;
    // �ӽ��ķ���
    [SerializeField] private Vector2 launchForce;
    // �ӳ�ȥ�Ľ������ܵ�����
    [SerializeField] private float swordGravity;
    // �����ӽ���ȥ�ķ���
    private Vector2 finalDir;

    [Header("Aim dots")]
    // ��׼�������
    [SerializeField] private int numberOfDots;
    // ÿ����׼���ľ���
    [SerializeField] private float spaceBetweenDots;
    // ��׼���Ԥ�Ƽ�
    [SerializeField] private GameObject dotPrefab;
    // ��׼��ĸ��࣬��Ҫ����������׼��ʱʹ��
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
            // normalized��ָ�����ĳ��ȣ����Ϊ��С��ģ��������Ϊ 1 ��������ͬʱ������ԭʼ�ķ��򲻱�
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
        // ��player��λ��ʵ����һ��sword����
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        // ��ʵ���������»�ȡ���Ʋ��
        SwordSkillController newSwordController = newSword.GetComponent<SwordSkillController>();
        // ���ý��ķ��䷽�������
        newSwordController.SetupSword(finalDir, swordGravity, player);
        player.AssignNewSword(newSword);
        // ���Ѿ������ȥ�ˣ�������׼�õĵ㲻�ɼ�
        DotsActive(false);
    }

    // ��ȡ��׼�ķ���
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        // ScreenToWorldPoint����Ļ�ϵĵ㣨���������Ļ�ϵ�������ĵ㣩ת��Ϊ����ռ��е������
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction;
    }

    // ����dot��������չʾ
    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    // ��dotPrefab��ʼ��dot����ʼλ��Ϊplayer����λ�ã�����ת
    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            // Quaternion.identity��ʾû����ת����Ԫ��
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParents);
            // Ĭ������dot
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
