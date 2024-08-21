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

        // 判断player的sword状态
        if(Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
        {
            // 鼠标右键，切换到aimSword
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
            // 鼠标左键，切换到攻击状态
            stateMachine.ChangeState(player.primaryAttackState);
            Debug.Log("player PlayerGroundedState change to primaryAttackState");
        }

        if(!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
            Debug.Log("player PlayerGroundedState change to airState");
        }

        // player.IsGroundDetected()确保player只有在地面的时候才可以跳，站在地面其他物体上不能跳
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
        // 如果剑不在手上，则回收剑
        player.sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }
}
