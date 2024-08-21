using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = .4f;
    private bool skillUsed;
    private float defaultGravity;

    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        skillUsed = false;
        stateTimer = flyTime;
        defaultGravity = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 15);
        }

        if(stateTimer <0)
        {
            rb.velocity = new Vector2(0, -.1f);

            if(!skillUsed)
            {
                if(player.skill.blackhole.CanUseSkill())
                {
                    skillUsed = true;
                }
            }
        }
        // 退出当前状态，当释放完技能后，在blackholeskillcontroller中调用函数退出
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = defaultGravity;
        player.MakeTransparent(false);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
