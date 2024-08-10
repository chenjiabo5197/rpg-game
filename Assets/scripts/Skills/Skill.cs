using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    // 技能的冷却时间
    [SerializeField] protected float cooldown;
    // 技能冷却时间的计时器
    protected float cooldownTimer;

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if(cooldownTimer < 0)
        {
            UseSkill();
            // 不在冷却时间内，重置冷却时间计时器
            cooldownTimer = cooldown;
            return true;
        }
        Debug.Log("skill is on cooldown");
        return false;
    }

    public virtual void UseSkill()
    {
        Debug.Log("use skill");
    }
}
