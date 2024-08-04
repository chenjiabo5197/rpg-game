using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyStatus
{
    private Enemy_Skeleton enemy;

    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _statusMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _statusMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        // »Øµ÷£¬Í£Ö¹¹¥»÷¶¯»­
        if(triggerCalled)
        {
            statusMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
