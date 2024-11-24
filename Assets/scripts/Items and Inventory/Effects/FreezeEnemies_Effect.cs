using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��unity�ڲ�����һ��create������Ŀ��·����Data/Item effect/Freeze enemies��������Ĭ����ΪFreeze enemies effect
[CreateAssetMenu(fileName = "Freeze enemies effect", menuName = "Data/Item effect/Freeze enemies")]
public class FreezeEnemies_Effect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * .1f)
        {
            // playerѪ������10%ʱ��armor�Ķ��᲻������
            return;
        }

        if (!Inventory.instance.CanUseArmor())
        {
            return;
        }

        // ��ȡ��ʱ�ڹ�����Χ����������
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);

        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}
