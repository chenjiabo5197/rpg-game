  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    // 攻击计数器，用于切换攻击
    private int comboCounter;
    // 最后一次的攻击时间
    private float lastTimeAttacked;
    // 多长时间内的攻击视为连击
    private float comboWindow = 2;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 超过连击时间或攻击次数大于攻击技能，重置计数器，下次攻击工attack1开始
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }
        player.anim.SetInteger("ComboCounter", comboCounter);

        // 用于切换连击之间攻击方向，单个攻击只能一个方向，两个攻击之间可以切换方向
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
        // 每次攻击后更新时间，用于判断下次攻击的时间在不在连击时间comboWindow内
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        // stateTimer在playerState中定义，继承该类的每个类中都会有一份拷贝，enter时，置为0，在调用update时，只更新本类中的stateTimer
        // 此时判断小于0，是进入攻击后第一次update即能满足该判断条件
        if (stateTimer < 0)
        {
            player.SetZeroVelocity();
        }

        // 判断是否要退出攻击状态
        if(triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
