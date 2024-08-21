using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.R))
        {
            stateMachine.ChangeState(player.blackholeState);
            Debug.Log("player PlayerGroundedState change to blackholeState");
        }

        // �ж�player��sword״̬
        if(Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
        {
            // ����Ҽ����л���aimSword
            stateMachine.ChangeState(player.aimSwordState);
            Debug.Log("player PlayerGroundedState change to aimSwordState");
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ChangeState(player.counterAttackState);
            Debug.Log("player PlayerGroundedState change to counterAttackState");
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            // ���������л�������״̬
            stateMachine.ChangeState(player.primaryAttackState);
            Debug.Log("player PlayerGroundedState change to primaryAttackState");
        }

        if(!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
            Debug.Log("player PlayerGroundedState change to airState");
        }

        // player.IsGroundDetected()ȷ��playerֻ���ڵ����ʱ��ſ�������վ�ڵ������������ϲ�����
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
            Debug.Log("player PlayerGroundedState change to jumpState");
        }
    }

    private bool HasNoSword()
    {
        if(!player.sword)
        {
            return true;
        }
        // ������������ϣ�����ս�
        player.sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }
}
