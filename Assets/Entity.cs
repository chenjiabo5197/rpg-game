using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

// player和enemy的基类
public class Entity : MonoBehaviour
{

    #region Compoents
    // 动画播放器，后面{}表示这个对象是一个只读的对象
    public Animator anim { get; private set; }
    // 代表实体的刚体
    public Rigidbody2D rb { get; private set; }

    #endregion
    // 碰撞的变量
    [Header("Collision info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] private float wallCheckDistance;

    // 朝向的方向，默认朝向右方
    public int facingDir { get; private set; } = 1;
    // 是否面朝右方，默认为true
    protected bool facingRight = true;

    // 在游戏对象被实例化时首先调用的方法
    protected virtual void Awake()
    {
        
    }

    // 在 Awake 方法之后被调用，用于在游戏对象启用后执行一次性初始化操作
    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // 游戏中每帧都会调用
    protected virtual void Update()
    {
        
    }

    #region Velocity
    // 将player的速度置为0
    public void SetZeroVelocity() => rb.velocity = new Vector2(0, 0);

    // 设置玩家的移动速度，包含x方向和y方向
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion

    #region Collision
    // 地面检测
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    // 墙面检测
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    // 画辅助线，目的是为了看到地面检测和墙体检测
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
    #endregion

    #region Flip
    // 反转函数，翻转player贴图
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        //Transform flipTransform = anim.GetComponentInParent<Transform>();
        //float newPositionX = transform.position.x;
        //float newPositionY = transform.position.y;
        //float newPositionZ = transform.position.z;
        transform.Rotate(0, 180, 0);
        //transform.position = new Vector3(newPositionX, newPositionY, newPositionZ);
    }

    // 翻转函数控制器，确认调用Flip函数的时间
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
        {
            Flip();
        }
        else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }
    #endregion
}
