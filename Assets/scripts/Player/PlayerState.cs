using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    // 设置rb对象，避免经常获取player.rb
    protected Rigidbody2D rb;

    // x方向上的输入
    protected float xInput;
    // y方向上的输入
    protected float yInput;
    // 动画名，主要用于设置切换动画的变量true、false，以用于切换动画
    private string animBoolName;
    // 定时器，用于状态切换使用（冲刺、walljump）
    protected float stateTimer;
    // 是否播放攻击动画
    protected bool triggerCalled;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        // 将动画中各自的bool值置为true，一般都是用这个值来进入此动画
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        // 进入所有状态都不会打断攻击动画的播放
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit()
    {
        // 将动画中各自的bool值置为false，一般都是用这个值来退出此动画，攻击动画除外(因为攻击动画一次需要完整播放完全部帧，所以在全部帧的后一帧调用函数退出攻击动画)
        player.anim.SetBool(animBoolName, false);
    }

    // 用于停止攻击动画的函数，在player中将此函数暴露出来，由animator在player中调用该函数来停止攻击，animator只能调用到player中
    public virtual void AnimationFinishTrigger()
    {
        // 停止攻击
        triggerCalled = true;
    }
}
