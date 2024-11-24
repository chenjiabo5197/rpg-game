using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 在unity内部新增一个create的新项目，路径是Data/Item effect/Freeze enemies，创建后默认名为Freeze enemies effect
[CreateAssetMenu(fileName = "Freeze enemies effect", menuName = "Data/Item effect/Freeze enemies")]
public class FreezeEnemies_Effect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * .1f)
        {
            // player血量大于10%时，armor的冻结不起作用
            return;
        }

        if (!Inventory.instance.CanUseArmor())
        {
            return;
        }

        // 获取此时在攻击球范围内所有物体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);

        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}
