using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// player�ķ���״̬�����ڴ�״̬ʱ��enemy������player�Ҵ���enemy�Ĺ��������׶Σ�enemyλ��player�Ĺ�����Χ�ڣ�player�ᷴ��enemy��
// �������ɹ�����enemy�Ĺ�����ʹenemy����ѣ��״̬
public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        // ��ȡ��ʱ��player������Χ����������
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            // ����б���������enemy
            if (hit.GetComponent<Enemy>() != null)
            {
                // �жϴ�ʱenemy�ܷ�ѣ�Σ������ԣ����stateTimerһ���ϴ��ֵ(ֵ���⣬Ŀ����Ϊ�˲��ù����ɹ��Ķ��������)��Ȼ����÷����ɹ��Ķ�������ʹenemy����ѣ��״̬
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10;
                    // ���ŷ����ɹ�����
                    player.anim.SetBool("SuccessfulCounterAttack", true);
                }
            }
        }
        // ����ڷ���ʱ����û�з����ɹ����ߴ�ʱ���������Ѳ����꣬���л�Ϊidle״̬
        if(stateTimer < 0 || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
