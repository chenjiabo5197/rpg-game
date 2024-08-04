using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    // ����rb���󣬱��⾭����ȡplayer.rb
    protected Rigidbody2D rb;

    // x�����ϵ�����
    protected float xInput;
    // y�����ϵ�����
    protected float yInput;
    // ����������Ҫ���������л������ı���true��false���������л�����
    private string animBoolName;
    // ��ʱ��������״̬�л�ʹ�ã���̡�walljump��
    protected float stateTimer;
    // �Ƿ񲥷Ź�������
    protected bool triggerCalled;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        // �������и��Ե�boolֵ��Ϊtrue��һ�㶼�������ֵ������˶���
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        // ��������״̬�������Ϲ��������Ĳ���
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
        // �������и��Ե�boolֵ��Ϊfalse��һ�㶼�������ֵ���˳��˶�����������������(��Ϊ��������һ����Ҫ����������ȫ��֡��������ȫ��֡�ĺ�һ֡���ú����˳���������)
        player.anim.SetBool(animBoolName, false);
    }

    // ����ֹͣ���������ĺ�������player�н��˺�����¶��������animator��player�е��øú�����ֹͣ������animatorֻ�ܵ��õ�player��
    public virtual void AnimationFinishTrigger()
    {
        // ֹͣ����
        triggerCalled = true;
    }
}
