using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundState
{
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _statusMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _statusMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            statusMachine.ChangeState(enemy.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
