using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    [Header("Attack details")]
    // 攻击中的移动,向上或向下
    public Vector2[] attackMovement;
    // player的反击时间
    public float counterAttackDuration = .2f;

    // 玩家目前状态，是否忙碌，目前用于停止连续攻击期间player的移动，因为每次player攻击结束后会进入idle状态，在idle中由于x轴有输入，所以会进入move状态，导致连击中会移动
    public bool isBusy { get; private set; }
    [Header("Move Info")]
    // player移动速度
    public float moveSpeed = 8f;
    // player向上跳起速度
    public float jumpForce;

    [Header("Dash Info")]
    // 冲刺冷却时间
    [SerializeField] private float dashCoolDown;
    // 冲刺冷却时间定时器
    private float dashUsageTimer;
    // player 冲刺速度
    public float dashSpeed;
    // player 冲刺时间
    public float dashDuration;
    // 冲刺方向
    public float dashDir { get; private set; }

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }     // airstate在动画中是fallstate，为啥不取一样名字呢
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }

    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    #endregion

    // 在游戏对象被实例化时首先调用的方法
    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");

        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
    }

    // 在 Awake 方法之后被调用，用于在游戏对象启用后执行一次性初始化操作
    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    // 游戏中每帧都会调用
    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        CheckForDashInput();
    }

    // 传入要等待的秒数，进入函数后，先将isBusy置为true，等待传入参数的秒数后，再将isBusy置为false，类似一个多线程的函数，等待秒数在多线程中计数，计数结束后，置为false
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    // Animator中调用该函数，停止攻击动画，animator只能调用到player中的函数
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    // 冲刺
    private void CheckForDashInput()
    {
        // 如果面对墙面则不冲刺，直接返回
        if (IsWallDetected())
        {
            return;
        }

        dashUsageTimer -= Time.deltaTime;

        // 如果按下冲刺按键，且不在冲刺冷却时间内，则进入冲刺状态
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCoolDown;
            // 从横坐标获取此时的朝向
            dashDir = Input.GetAxisRaw("Horizontal");
            if(dashDir == 0)
            {
                dashDir = facingDir;
            }
            stateMachine.ChangeState(dashState);   
        }
    }
}
