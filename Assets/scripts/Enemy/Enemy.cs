using System.Collections;
using UnityEngine;

// �����ͬ���˵Ļ��࣬����skeleton
public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Stunned Info")]
    // ����ѣ��״̬��ʱ��
    public float stunDuration;
    public Vector2 stunDirection;
    // �ж��Ƿ���ѣ��״̬
    protected bool canBeStunned;
    // enemy�Ĺ�������(����������������ħ)������enemyͷ�����к��
    [SerializeField] protected GameObject counterImage;

    [Header("Move Info")]
    public float moveSpeed;
    // ����ʱ�䣬����ʱ���ʱ����������˶�״̬
    public float idleTime;
    // ս��ʱ�䣬ս��ʱ���ʱ���������idle״̬
    public float battleTime;
    // Ĭ�ϵ��ƶ��ٶ�
    private float defaultMoveSpeed;

    [Header("Attack Info")]
    public float attackDistance;
    public float attackCoolDown;
    // ǰ׺������inspector�����ظ���Ŀ����Ȼ����ĿΪpublic����
    [HideInInspector] public float lastTimeAttacked;

    // ״̬��
    public EnemyStateMachine stateMachine { get; private set; }
    // ��¼enemy���һ��������
    public string lastAnimBoolName { get; private set; }

    // ����Ϸ����ʵ����ʱ���ȵ��õķ���
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
    // �򿪹������ڣ�չʾcounterImage
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    // �رչ������ڣ�ȡ��չʾcounterImage
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        // ����counterImage
        counterImage.SetActive(false);
    }
    #endregion

    // �ж�enemy�Ƿ��ܱ�ѣ��,enemy���ڴ򿪹������ڽ׶��ǿ��Ա�ѣ�εģ�����ʱ��㲻�ɱ�ѣ��
    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    // Animator�е��øú�����ֹͣ����������animatorֻ�ܵ��õ�Enemy�еĺ���
    public virtual void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D isPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x * attackDistance * facingDir, transform.position.y));
    }
}
