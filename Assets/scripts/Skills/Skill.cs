using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    // 技能的冷却时间
    [SerializeField] protected float cooldown;
    // 技能冷却时间的计时器
    protected float cooldownTimer;

    protected Player player;

    private void Awake()
    {
        
    }

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
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

    // 获取距离_checkTransform最近的enemy
    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        Transform closestEnemy = null;
        // 获取此时在25范围内所有物体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);
        // 正无穷大
        float closestDistance = Mathf.Infinity;

        foreach (var hit in colliders)
        {
            // 循环获取离clone体最近的enemy对象
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestEnemy = hit.transform;
                    closestDistance = distanceToEnemy;
                }
            }
        }
        return closestEnemy;
    }
}
