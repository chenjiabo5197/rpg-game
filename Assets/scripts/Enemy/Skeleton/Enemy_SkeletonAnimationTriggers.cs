using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkeletonAnimationTriggers : MonoBehaviour
{
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    // 为了调用EnemySkeleton中的AnimationTrigger函数，将triggerCalled变量置为true，停止攻击，一般在攻击动画的后一帧调用
    private void AnimationTrigger()
    {
        // 一般是animator中的插件调用，但是EnemySkeleton是animator的父节点，所以要先从父类中获取到EnemySkeleton，然后再调用EnemySkeleton函数，triggerCalled置为true
        enemy.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach(var hit in colliders)
        {
            // 如果enemy此时攻击范围内有player，代用player的damage函数，表示player收到伤害
            if(hit.GetComponent<Player>() != null)
            {
                hit.GetComponent<Player>().Damage();
            }
        }
    }
}
