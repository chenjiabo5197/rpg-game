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
        // 乘负的面向方向目的是为了跳的时候向远离墙的方向跳，因为一般wallslide状态的facingDir是面向墙壁的
        player.SetVelocity(5 * -player.facingDir, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // wallJump时间结束后，player可能处于空中，所以应该改为airstate状态(上升已经结束，只能处于下落状态)
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.airState);
        }

        // 下面这段代码在wallJump时检测地面碰撞会出现前几帧检测位于地面的误检测情况，原因不明，所以不使用这种方式转化为idlestate，而是调整stateTimer的值
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
