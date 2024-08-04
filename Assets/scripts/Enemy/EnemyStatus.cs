using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus
{
    protected EnemyStateMachine stateMachine;
    // Enemy基类
    protected Enemy enemyBase;
    protected Rigidbody2D rb;

    // 是否要停止攻击动画
    protected bool triggerCalled;
    // 动画名，用于切换是否播放动画
    private string animBoolName;

    // 状态计时器
    protected float stateTimer;

    public EnemyStatus(Enemy _enemyBase, EnemyStateMachine _statusMachine, string _animBoolName)
    {
        this.stateMachine = _statusMachine;
        this.enemyBase = _enemyBase;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        rb = enemyBase.rb;

        enemyBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
    }

    // 用于停止攻击动画的函数，在EnemySkeleton中将此函数暴露出来，由animator在EnemySkeleton中调用该函数来停止攻击，animator只能调用到EnemySkeleton中
    public virtual void AnimationFinishTrigger()
    {
        // 停止攻击
        triggerCalled = true;
    }
}
