 using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

// player��enemy�Ļ���
public class Entity : MonoBehaviour
{

    #region Compoents
    // ����������������{}��ʾ���������һ��ֻ���Ķ���
    public Animator anim { get; private set; }
    // ����ʵ��ĸ���
    public Rigidbody2D rb { get; private set; }
    // ʵ�屻����ʱ��չʾЧ��
    public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CharacterStats stats { get; private set; }
    // ��ײ��
    public CapsuleCollider2D cc { get; private set; }
    #endregion

    [Header("Knockback Info")]
    // �����˵ķ���
    [SerializeField] protected Vector2 knockbackDirection;
    // ������ά�ֵ�ʱ��
    [SerializeField] protected float knockbackDuration;
    // �ж��Ƿ񱻻���
    protected bool isKnocked;

    // ��ײ�ı���
    [Header("Collision info")]
    public Transform attackCheck;
    // ���������İ뾶
    public float attackCheckRadius;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] private float wallCheckDistance;

    // ����ķ���Ĭ�ϳ����ҷ����ҷ�ֵΪ1����ֵΪ-1
    public int facingDir { get; private set; } = 1;
    // �Ƿ��泯�ҷ���Ĭ��Ϊtrue
    protected bool facingRight = true;

    public System.Action onFlipped;

    // ����Ϸ����ʵ����ʱ���ȵ��õķ���
    protected virtual void Awake()
    {
        // ����player��enemy����ͼ�����ײ(ȫ�ּ���)��playerͼ������6��enemyͼ������7����Ҫ�������������ײ��Ҫ�ٴε��øú�����������ͬ�㼶��Ȼ�����������Ϊfalse����
        Physics2D.IgnoreLayerCollision(6, 7);
        Physics2D.IgnoreLayerCollision(7, 7);
        // swordͼ������8
        Physics2D.IgnoreLayerCollision(6, 8);

        // ����Item���������֮�����в㼶����ײ
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

    // �� Awake ����֮�󱻵��ã���������Ϸ�������ú�ִ��һ���Գ�ʼ������
    protected virtual void Start()
    {
        
    }

    // ��Ϸ��ÿ֡�������
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

    // �˺�������player��enemy���øú������������յ����˺�  // ��һ��Э�̣������к����
    public virtual void DamageImpact() => StartCoroutine("HisKnockback");

    // ���˺�����Э�̵��øú���
    protected virtual IEnumerator HisKnockback()
    {
        isKnocked  = true;

        rb.velocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }

    #region Velocity
    // ���ٶ���Ϊ0
    public void SetZeroVelocity()
    {
        if (isKnocked)
        {
            // ���ڱ�����״̬���������ٶ�
            return;
        }

        rb.velocity = new Vector2(0, 0);
    }

    // ������ҵ��ƶ��ٶȣ�����x�����y����
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if(isKnocked)
        {
            // ���ڱ�����״̬���������ٶ�
            return;
        }

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion

    #region Collision
    // ������
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    // ǽ����
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    // ��������
    protected virtual void OnDrawGizmos()
    {
        // ��������
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        // ǽ������
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        // �����������
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip
    // ��ת��������תplayer��ͼ
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

    // ��ת������������ȷ�ϵ���Flip������ʱ��
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
