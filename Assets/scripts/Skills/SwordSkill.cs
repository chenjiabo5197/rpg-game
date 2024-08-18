using System;
using UnityEngine;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class SwordSkill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce info")]
    // ������������
    [SerializeField] private int bounceAmount;
    // �����Ľ�������
    [SerializeField] private float bounceGravity;
    // ���ķ����ٶ�
    [SerializeField] private float bounceSpeed;

    [Header("Pierce info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin info")]
    [SerializeField] private float hitCooldown = .35f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;
    [SerializeField] private float returnSpeed;

    [Header("Skill Info")]
    // ����Ԥ���壬������Ⱦ���ӳ�ȥ����з���
    [SerializeField] private GameObject swordPrefab;
    // �ӽ��ķ���
    [SerializeField] private Vector2 launchForce;
    // �ӳ�ȥ�Ľ������ܵ�����
    [SerializeField] private float swordGravity;
    // �ӳ�ȥ�Ľ�����enemy�󶳽�enemy��ʱ��
    [SerializeField] private float freezeTimeDuration;

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

        SetupGravity();
    }

    private void SetupGravity()
    {
        if (swordType == SwordType.Bounce)
        {
            swordGravity = bounceGravity;
        }
        else if (swordType == SwordType.Pierce)
        {
            swordGravity = pierceGravity;
        }
        else if (swordType == SwordType.Spin)
        {
            swordGravity = spinGravity;
        }
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
            for (int i = 0; i < dots.Length; i++)
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

        if (swordType == SwordType.Bounce)
        {
            newSwordController.SetupBounce(true, bounceAmount, bounceSpeed);
        }
        else if (swordType == SwordType.Pierce)
        {
            newSwordController.SetupPierce(pierceAmount);
        }
        else if (swordType == SwordType.Spin)
        {
            newSwordController.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);
        }

        // ���ý��ķ��䷽�������
        newSwordController.SetupSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);
        player.AssignNewSword(newSword);
        // ���Ѿ������ȥ�ˣ�������׼�õĵ㲻�ɼ�
        DotsActive(false);
    }

    #region Aim region
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
    #endregion
}
