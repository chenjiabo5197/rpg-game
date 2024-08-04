using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonGroundState : EnemyStatus
{
    protected Enemy_Skeleton enemy;
    // player此时的坐标
    protected Transform player;

    public SkeletonGroundState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        // 判断enemy探测到player或者enemy与player的距离小于2，则变为战斗状态
        if(enemy.isPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < 2)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
