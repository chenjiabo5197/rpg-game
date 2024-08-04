using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        // if (xInput == 0 || player.IsWallDetected())   区别在于加上之后的目的是player在墙壁前是idle状态，按键无反应，但是会造成idle与move频繁切换，player的动画跳变
        if (xInput == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
