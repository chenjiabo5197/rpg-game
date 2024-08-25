using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    // 力量
    public Stat strength;
    // 攻击力
    public Stat damage;
    // 最大血量
    public Stat maxHealth;

    [SerializeField] private int currentHealth;

    public virtual void Start()
    {
        currentHealth = maxHealth.GetValue();

    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        int totalDamage = damage.GetValue() + strength.GetValue();
        _targetStats.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;

        if (currentHealth < 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {

    }
}
