using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    // Ϊ�˵���player�е�AnimationTrigger��������triggerCalled������Ϊtrue��ֹͣ������һ���ڹ��������ĺ�һ֡����
    private void AnimationTrigger()
    {
        // һ����animator�еĲ�����ã�����player��animator�ĸ��ڵ㣬����Ҫ�ȴӸ����л�ȡ��player��Ȼ���ٵ���player������triggerCalled��Ϊtrue����playerstate��ֹͣ��������
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        // ��ȡ��ʱ��player������Χ����������
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            // ����б���������enemy�������enemy��damage��������ʾenemy�յ��˺�
            if (hit.GetComponent<Enemy>() != null)
            {
                /*hit.GetComponent<Enemy>().Damage();
                hit.GetComponent<CharacterStats>().TakeDamage(player.stats.damage.GetValue());*/
                EnemyStats target = hit.GetComponent<EnemyStats>();
                player.stats.DoDamage(target);
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
