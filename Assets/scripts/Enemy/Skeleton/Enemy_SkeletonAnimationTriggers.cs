using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkeletonAnimationTriggers : MonoBehaviour
{
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    // Ϊ�˵���EnemySkeleton�е�AnimationTrigger��������triggerCalled������Ϊtrue��ֹͣ������һ���ڹ��������ĺ�һ֡����
    private void AnimationTrigger()
    {
        // һ����animator�еĲ�����ã�����EnemySkeleton��animator�ĸ��ڵ㣬����Ҫ�ȴӸ����л�ȡ��EnemySkeleton��Ȼ���ٵ���EnemySkeleton������triggerCalled��Ϊtrue
        enemy.AnimationTrigger();
    }

    // �����ص��������ڶ�����ĳһ֡���øú������жϴ�ʱҪ�����Ķ����ڲ��ڹ�����Χ�ڣ������ڣ����������˺�
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach(var hit in colliders)
        {
            // ���enemy��ʱ������Χ����player������player��damage��������ʾplayer�յ��˺�
            if(hit.GetComponent<Player>() != null)
            {
                /*hit.GetComponent<Player>().Damage();*/
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(target);
            }
        }
    }

    // �ص��������ֱ���ö�����enemy�еĺ������򿪺͹رչ�������
    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
