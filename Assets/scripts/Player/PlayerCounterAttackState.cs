using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// player的反击状态，处于此状态时，enemy若攻击player且处于enemy的攻击蓄力阶段，enemy位于player的攻击范围内，player会反击enemy，
// 若反击成功则打断enemy的攻击，使enemy处于眩晕状态
public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        // 获取此时在player攻击球范围内所有物体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            // 如果列表中物体是enemy
            if (hit.GetComponent<Enemy>() != null)
            {
                // 判断此时enemy能否被眩晕，若可以，则给stateTimer一个较大的值(值随意，目的是为了不让攻击成功的动画被打断)，然后调用反击成功的动画，并使enemy处于眩晕状态
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10;
                    // 播放反击成功动画
                    player.anim.SetBool("SuccessfulCounterAttack", true);
                }
            }
        }
        // 如果在反击时间内没有反击成功或者此时攻击动画已播放完，则切换为idle状态
        if(stateTimer < 0 || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
