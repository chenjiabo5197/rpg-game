using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void DoDamage(CharacterStats _targetStats)
    {
        base.DoDamage(_targetStats);

    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        player.DamageEffect();
    }

    protected override void Dead()
    {
        base.Dead();

        player.Dead();
    }
}
