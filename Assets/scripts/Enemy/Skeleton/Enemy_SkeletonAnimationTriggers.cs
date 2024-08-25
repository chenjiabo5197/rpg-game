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

    // 攻击回调函数，在动画的某一帧调用该函数，判断此时要攻击的对象在不在攻击范围内，若在内，则对其造成伤害
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach(var hit in colliders)
        {
            // 如果enemy此时攻击范围内有player，代用player的damage函数，表示player收到伤害
            if(hit.GetComponent<Player>() != null)
            {
                /*hit.GetComponent<Player>().Damage();*/
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(target);
            }
        }
    }

    // 回调函数，分别调用定义在enemy中的函数，打开和关闭攻击窗口
    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
