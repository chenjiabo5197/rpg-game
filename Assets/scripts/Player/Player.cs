using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    [Header("Attack details")]
    // �����е��ƶ�,���ϻ�����
    public Vector2[] attackMovement;
    // player�ķ���ʱ��
    public float counterAttackDuration = .2f;

    // ���Ŀǰ״̬���Ƿ�æµ��Ŀǰ����ֹͣ���������ڼ�player���ƶ�����Ϊÿ��player��������������idle״̬����idle������x�������룬���Ի����move״̬�����������л��ƶ�
    public bool isBusy { get; private set; }
    [Header("Move Info")]
    // player�ƶ��ٶ�
    public float moveSpeed = 8f;
    // player���������ٶ�
    public float jumpForce;

    [Header("Dash Info")]
    // �����ȴʱ��
    [SerializeField] private float dashCoolDown;
    // �����ȴʱ�䶨ʱ��
    private float dashUsageTimer;
    // player ����ٶ�
    public float dashSpeed;
    // player ���ʱ��
    public float dashDuration;
    // ��̷���
    public float dashDir { get; private set; }

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }     // airstate�ڶ�������fallstate��Ϊɶ��ȡһ��������
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }

    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    #endregion

    // ����Ϸ����ʵ����ʱ���ȵ��õķ���
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

    // �� Awake ����֮�󱻵��ã���������Ϸ�������ú�ִ��һ���Գ�ʼ������
    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    // ��Ϸ��ÿ֡�������
    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        CheckForDashInput();
    }

    // ����Ҫ�ȴ������������뺯�����Ƚ�isBusy��Ϊtrue���ȴ�����������������ٽ�isBusy��Ϊfalse������һ�����̵߳ĺ������ȴ������ڶ��߳��м�����������������Ϊfalse
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    // Animator�е��øú�����ֹͣ����������animatorֻ�ܵ��õ�player�еĺ���
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    // ���
    private void CheckForDashInput()
    {
        // ������ǽ���򲻳�̣�ֱ�ӷ���
        if (IsWallDetected())
        {
            return;
        }

        dashUsageTimer -= Time.deltaTime;

        // ������³�̰������Ҳ��ڳ����ȴʱ���ڣ��������״̬
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCoolDown;
            // �Ӻ������ȡ��ʱ�ĳ���
            dashDir = Input.GetAxisRaw("Horizontal");
            if(dashDir == 0)
            {
                dashDir = facingDir;
            }
            stateMachine.ChangeState(dashState);   
        }
    }
}
