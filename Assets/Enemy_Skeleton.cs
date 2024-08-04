using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// skeleton类enemy的基类，继承enemy
public class Enemy_Skeleton : Enemy
{

    #region States

    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        
        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);

        battleState = new SkeletonBattleState(this, stateMachine, "Move", this);
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    // Animator中调用该函数，停止攻击动画，animator只能调用到EnemySkeleton中的函数
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
