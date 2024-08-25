using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private Enemy_Skeleton enemy;

    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _statusMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _statusMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        // �Թ̶���ʱ�����ظ�����һ������������1�ظ����õķ��������ƣ�����2���״ε��÷���֮ǰ�ȴ�������������3֮��ÿ�ε��÷���֮���ʱ����
        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);

        stateTimer = enemy.stunDuration;
        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        // ������ָ����ʱ������һ������������1��ָ��ʱ�����õķ��������ƣ�����2�ڵ��÷���֮ǰ��Ҫ�ȴ�������
        enemy.fx.Invoke("CancelRedBlink", 0);
    }
}
