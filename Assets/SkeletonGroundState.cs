using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonGroundState : EnemyStatus
{
    protected Enemy_Skeleton enemy;

    public SkeletonGroundState(Enemy _enemyBase, EnemyStateMachine _statusMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _statusMachine, _animBoolName)
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

        if(enemy.isPlayerDetected())
        {
            statusMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
