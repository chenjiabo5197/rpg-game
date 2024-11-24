using System.Collections;
using UnityEngine;

// 多个不同敌人的基类，包含skeleton
public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Stunned Info")]
    // 处于眩晕状态的时间
    public float stunDuration;
    public Vector2 stunDirection;
    // 判断是否处于眩晕状态
    protected bool canBeStunned;
    // enemy的攻击动画(看起来类似武器附魔)攻击中enemy头部会有红框
    [SerializeField] protected GameObject counterImage;

    [Header("Move Info")]
    public float moveSpeed;
    // 空闲时间，空闲时间计时结束后进入运动状态
    public float idleTime;
    // 战斗时间，战斗时间计时结束后进入idle状态
    public float battleTime;
    // 默认的移动速度
    private float defaultMoveSpeed;

    [Header("Attack Info")]
    public float attackDistance;
    public float attackCoolDown;
    // 前缀表明在inspector中隐藏该项目，虽然该项目为public属性
    [HideInInspector] public float lastTimeAttacked;

    // 状态机
    public EnemyStateMachine stateMachine { get; private set; }
    // 记录enemy最后一个动画名
    public string lastAnimBoolName { get; private set; }

    // 在游戏对象被实例化时首先调用的方法
    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();

        defaultMoveSpeed = moveSpeed;
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

    }

    public virtual void AssignLastAnimName(string _animBoolName) => lastAnimBoolName = _animBoolName;

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowDuration);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    public virtual void FreezeTimeFor(float _duration) => StartCoroutine(FreezeTimerCoroutine(_duration));

    protected virtual IEnumerator FreezeTimerCoroutine(float _seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(_seconds);

        FreezeTime(false);
    }

    #region Counter Attack Window
    // 打开攻击窗口，展示counterImage
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    // 关闭攻击窗口，取消展示counterImage
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        // 隐藏counterImage
        counterImage.SetActive(false);
    }
    #endregion

    // 判断enemy是否能被眩晕,enemy处于打开攻击窗口阶段是可以被眩晕的，其余时间点不可被眩晕
    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    // Animator中调用该函数，停止攻击动画，animator只能调用到Enemy中的函数
    public virtual void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D isPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x * attackDistance * facingDir, transform.position.y));
    }
}
