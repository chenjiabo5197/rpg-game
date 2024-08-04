using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus
{
    protected EnemyStateMachine stateMachine;
    // Enemy����
    protected Enemy enemyBase;
    protected Rigidbody2D rb;

    // �Ƿ�Ҫֹͣ��������
    protected bool triggerCalled;
    // �������������л��Ƿ񲥷Ŷ���
    private string animBoolName;

    // ״̬��ʱ��
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

    // ����ֹͣ���������ĺ�������EnemySkeleton�н��˺�����¶��������animator��EnemySkeleton�е��øú�����ֹͣ������animatorֻ�ܵ��õ�EnemySkeleton��
    public virtual void AnimationFinishTrigger()
    {
        // ֹͣ����
        triggerCalled = true;
    }
}
