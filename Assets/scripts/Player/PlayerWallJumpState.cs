using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = .8f;
        // �˸���������Ŀ����Ϊ������ʱ����Զ��ǽ�ķ���������Ϊһ��wallslide״̬��facingDir������ǽ�ڵ�
        player.SetVelocity(5 * -player.facingDir, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // wallJumpʱ�������player���ܴ��ڿ��У�����Ӧ�ø�Ϊairstate״̬(�����Ѿ�������ֻ�ܴ�������״̬)
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.airState);
        }

        // ������δ�����wallJumpʱ��������ײ�����ǰ��֡���λ�ڵ�������������ԭ���������Բ�ʹ�����ַ�ʽת��Ϊidlestate�����ǵ���stateTimer��ֵ
        /*if(!player.IsGroundDetected())
        {
            Debug.Log("not ground:"+stateTimer);
            // stateMachine.ChangeState(player.idleState);
        }

        if (player.IsGroundDetected())
        {
            Debug.Log("ground:"+stateTimer);
            // stateMachine.ChangeState(player.idleState);
        }*/
    }
}
