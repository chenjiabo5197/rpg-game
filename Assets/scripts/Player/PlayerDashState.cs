using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 创建一个player对象的clone体
        //SkillManager.instance.clone.CreateClone(player.transform);
        player.skill.clone.CreateClone(player.transform, Vector3.zero);

        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();
        
        if(!player.IsGroundDetected() && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        player.SetVelocity(player.dashDir * player.dashSpeed, 0);

        // 冲刺时间结束后，切换为idle状态
        if(stateTimer < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
