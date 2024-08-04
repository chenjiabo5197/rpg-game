using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if(player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        if(player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }

        // 让player在下落中可以转换方向
        if(xInput != 0)
        {
            player.SetVelocity(xInput * .8f * player.moveSpeed, rb.velocity.y);
        }
    }
}
