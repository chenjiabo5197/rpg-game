using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonBattleState : EnemyStatus
{
    // player的坐标
    private Transform player;
    private Enemy_Skeleton enemy;
    // enemy的移动方向
    private int moveDir;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
            // 重置状态定时器
            stateTimer = enemy.battleTime;
            if (enemy.isPlayerDetected().distance < enemy.attackDistance)
            {
                if(CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
            }
        }
        else
        {
            // 长时间未检测到player，或者与player的距离过长，恢复到idle状态
            if(stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        if (player.position.x > enemy.transform.position.x)
        {
            // player在enemy的右侧
            moveDir = 1;
        }
        else if (player.position.x < enemy.transform.position.x)
        {
            // player在enemy的左侧
            moveDir = -1;
        }

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    // 判断当前时间是否在攻击冷却时间内，是否可以攻击
    public bool CanAttack()
    {
        if(Time.time >= enemy.lastTimeAttacked + enemy.attackCoolDown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}
