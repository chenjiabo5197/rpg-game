using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    // SkillManager�ĵ�������
    public static SkillManager instance;

    public DashSkill dash { get; private set; }
    // clone���ܣ�����player����
    public CloneSkill clone { get; private set; }
    public SwordSkill sword { get; private set; }
    public BlackholeSkill blackhole { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        dash = GetComponent<DashSkill>();
        clone = GetComponent<CloneSkill>();
        sword = GetComponent<SwordSkill>();
        blackhole = GetComponent<BlackholeSkill>();
    }
}
