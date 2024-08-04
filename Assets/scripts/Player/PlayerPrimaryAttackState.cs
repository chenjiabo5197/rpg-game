  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    // �����������������л�����
    private int comboCounter;
    // ���һ�εĹ���ʱ��
    private float lastTimeAttacked;
    // �೤ʱ���ڵĹ�����Ϊ����
    private float comboWindow = 2;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // ��������ʱ��򹥻��������ڹ������ܣ����ü��������´ι�����attack1��ʼ
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }
        player.anim.SetInteger("ComboCounter", comboCounter);

        // �����л�����֮�乥�����򣬵�������ֻ��һ��������������֮������л�����
        float attackDir = player.facingDir;
        if(xInput != 0)
        {
            attackDir = xInput;
        }

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .15f);

        comboCounter++;
        // ÿ�ι��������ʱ�䣬�����ж��´ι�����ʱ���ڲ�������ʱ��comboWindow��
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        // stateTimer��playerState�ж��壬�̳и����ÿ�����ж�����һ�ݿ�����enterʱ����Ϊ0���ڵ���updateʱ��ֻ���±����е�stateTimer
        // ��ʱ�ж�С��0���ǽ��빥�����һ��update����������ж�����
        if (stateTimer < 0)
        {
            player.SetZeroVelocity();
        }

        // �ж��Ƿ�Ҫ�˳�����״̬
        if(triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
