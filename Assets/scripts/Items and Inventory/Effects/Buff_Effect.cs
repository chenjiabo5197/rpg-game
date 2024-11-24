using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

// 可以增加buff的种类
public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChange,
    critPower,
    maxHealth,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightingDamage
}

// 在unity内部新增一个create的新项目，路径是Data/Item effect/Buff，创建后默认名为Buff effect
[CreateAssetMenu(fileName = "Buff effect", menuName = "Data/Item effect/Buff effect")]
public class Buff_Effect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        stats.IncreaseStatBy(buffAmount, buffDuration, StatToModify());
    }

    private Stat StatToModify()
    {
        switch (buffType)
        {
            case StatType.strength:
                return stats.strength;
            case StatType.agility:
                return stats.agility;
            case StatType.intelligence:
                return stats.intelligence;
            case StatType.vitality:
                return stats.vitality;
            case StatType.damage:
                return stats.damage;
            case StatType.critChange:
                return stats.critChange;
            case StatType.maxHealth:
                return stats.maxHealth;
            case StatType.armor:
                return stats.armor;
            case StatType.evasion:
                return stats.evasion;
            case StatType.magicResistance:
                return stats.magicResistance;
            case StatType.fireDamage:
                return stats.fireDamage;
            case StatType.iceDamage:
                return stats.iceDamage;
            case StatType.lightingDamage:
                return stats.lightingDamage;
            default:
                Debug.Log("unknown type");
                return null;
        }
    }
}
