using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// �����ͬ���˵Ļ��࣬����skeleton
public class Enemy : Entity
{
    [SerializeField]protected LayerMask whatIsPlayer;

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

    [Header("Attack Info")]
    public float attackDistance;
    public float attackCoolDown;
    // ǰ׺������inspector�����ظ���Ŀ����Ȼ����ĿΪpublic����
    [HideInInspector]public float lastTimeAttacked;

    // ״̬��
    public EnemyStateMachine stateMachine { get; private set; }

    // ����Ϸ����ʵ����ʱ���ȵ��õķ���
    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
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
