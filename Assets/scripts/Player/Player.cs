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
    // player �ڻ��ս�ʱ�������ĳ����
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

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
    private float defaultDashSpeed;

    // SkillManager��ʵ�������󣬺������ʸö��󲻱�ͨ��SkillManager.instance
    public SkillManager skill { get; private set; }
    // ����sword���󣬷�ֹ�ӳ�ȥ��ѽ�
    public GameObject sword { get; private set; }

    #region States
    // player�ĸ���״̬
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

    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackholeState blackholeState { get; private set; }

    public PlayerDeadState deadState { get; private set; }
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

        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackholeState = new PlayerBlackholeState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Dead");
    }

    // �� Awake ����֮�󱻵��ã���������Ϸ�������ú�ִ��һ���Գ�ʼ������
    protected override void Start()
    {
        base.Start();

        skill = SkillManager.instance;

        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    // ��Ϸ��ÿ֡�������
    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        CheckForDashInput();

        if(Input.GetKeyDown(KeyCode.F))
        {
            skill.crystal.CanUseSkill();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.instance.UseFlask();
        }
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    // ���������sword����
    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Debug.Log("change to catchSwordState");
        Destroy(sword);
    }

    public void ExitBlackholeAbility()
    {
        stateMachine.ChangeState(airState);
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

        // ������³�̰������Ҳ��ڳ����ȴʱ���ڣ��������״̬
        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            // �Ӻ������ȡ��ʱ�ĳ���
            dashDir = Input.GetAxisRaw("Horizontal");
            if(dashDir == 0)
            {
                dashDir = facingDir;
            }
            stateMachine.ChangeState(dashState);   
        }
    }

    public override void Dead()
    {
        base.Dead();

        stateMachine.ChangeState(deadState);
    }
}
