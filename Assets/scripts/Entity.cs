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
    // 实体被击中时的展示效果
    public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CharacterStats stats { get; private set; }
    // 碰撞体
    public CapsuleCollider2D cc { get; private set; }
    #endregion

    [Header("Knockback Info")]
    // 被击退的方向
    [SerializeField] protected Vector2 knockbackDirection;
    // 被击退维持的时间
    [SerializeField] protected float knockbackDuration;
    // 判断是否被击退
    protected bool isKnocked;

    // 碰撞的变量
    [Header("Collision info")]
    public Transform attackCheck;
    // 攻击检测球的半径
    public float attackCheckRadius;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] private float wallCheckDistance;

    // 朝向的方向，默认朝向右方，右方值为1，左方值为-1
    public int facingDir { get; private set; } = 1;
    // 是否面朝右方，默认为true
    protected bool facingRight = true;

    public System.Action onFlipped;

    // 在游戏对象被实例化时首先调用的方法
    protected virtual void Awake()
    {
        // 忽略player与enemy两个图层的碰撞(全局级别)，player图层编号是6，enemy图层编号是7，想要撤销这个忽略碰撞需要再次调用该函数，传入相同层级，然后第三个参数为false即可
        Physics2D.IgnoreLayerCollision(6, 7);
        Physics2D.IgnoreLayerCollision(7, 7);
        // sword图层编号是8
        Physics2D.IgnoreLayerCollision(6, 8);

        // 忽略Item与除过地面之外所有层级的碰撞
        Physics2D.IgnoreLayerCollision(9, 0);
        Physics2D.IgnoreLayerCollision(9, 1);
        Physics2D.IgnoreLayerCollision(9, 2);
        Physics2D.IgnoreLayerCollision(9, 4);
        Physics2D.IgnoreLayerCollision(9, 5);
        Physics2D.IgnoreLayerCollision(9, 6);
        Physics2D.IgnoreLayerCollision(9, 7);
        Physics2D.IgnoreLayerCollision(9, 8);

        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFX>();
        sr = GetComponentInChildren<SpriteRenderer>();  
        stats = GetComponent<CharacterStats>();
        cc = GetComponent<CapsuleCollider2D>();
    }

    // 在 Awake 方法之后被调用，用于在游戏对象启用后执行一次性初始化操作
    protected virtual void Start()
    {
        
    }

    // 游戏中每帧都会调用
    protected virtual void Update()
    {
        
    }

    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {

    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    // 伤害函数，player与enemy调用该函数，表明其收到了伤害  // 起一个协程，被击中后后退
    public virtual void DamageImpact() => StartCoroutine("HisKnockback");

    // 击退函数，协程调用该函数
    protected virtual IEnumerator HisKnockback()
    {
        isKnocked  = true;

        rb.velocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }

    #region Velocity
    // 将速度置为0
    public void SetZeroVelocity()
    {
        if (isKnocked)
        {
            // 处于被击退状态，不设置速度
            return;
        }

        rb.velocity = new Vector2(0, 0);
    }

    // 设置玩家的移动速度，包含x方向和y方向
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if(isKnocked)
        {
            // 处于被击退状态，不设置速度
            return;
        }

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion

    #region Collision
    // 地面检测
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    // 墙面检测
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    // 画辅助线
    protected virtual void OnDrawGizmos()
    {
        // 地面检测线
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        // 墙体检测线
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        // 画攻击检测球
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
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

        if(onFlipped != null)
        {
            onFlipped();
        }
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

    public virtual void Dead()
    {
    }
}
