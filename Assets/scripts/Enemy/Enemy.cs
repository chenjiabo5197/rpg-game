using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 多个不同敌人的基类，包含skeleton
public class Enemy : Entity
{
    [SerializeField]protected LayerMask whatIsPlayer;

    [Header("Stunned Info")]
    // 处于眩晕状态的时间
    public float stunDuration;
    public Vector2 stunDirection;

    [Header("Move Info")]
    public float moveSpeed;
    // 空闲时间，空闲时间计时结束后进入运动状态
    public float idleTime;
    // 战斗时间，战斗时间计时结束后进入idle状态
    public float battleTime;

    [Header("Attack Info")]
    public float attackDistance;
    public float attackCoolDown;
    // 前缀表明在inspector中隐藏该项目，虽然该项目为public属性
    [HideInInspector]public float lastTimeAttacked;

    // 状态机
    public EnemyStateMachine stateMachine { get; private set; }

    // 在游戏对象被实例化时首先调用的方法
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

    public virtual RaycastHit2D isPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x * attackDistance * facingDir, transform.position.y));
    }
}
