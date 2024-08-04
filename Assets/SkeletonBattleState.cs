using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyStatus
{
    // player������
    private Transform player;
    private Enemy_Skeleton enemy;
    // enemy���ƶ�����
    private int moveDir;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _statusMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _statusMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Player").transform;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.isPlayerDetected())
        {
            if (enemy.isPlayerDetected().distance < enemy.attackDistance)
            {
                statusMachine.ChangeState(enemy.attackState);
            }
        }

        if (player.position.x > enemy.transform.position.x)
        {
            // player��enemy���Ҳ�
            moveDir = 1;
        }
        else if (player.position.x < enemy.transform.position.x)
        {
            // player��enemy�����
            moveDir = -1;
        }

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
