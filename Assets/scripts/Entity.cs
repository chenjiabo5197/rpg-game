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

    #endregion
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

    // ����ķ���Ĭ�ϳ����ҷ�
    public int facingDir { get; private set; } = 1;
    // �Ƿ��泯�ҷ���Ĭ��Ϊtrue
    protected bool facingRight = true;

    // ����Ϸ����ʵ����ʱ���ȵ��õķ���
    protected virtual void Awake()
    {
        
    }

    // �� Awake ����֮�󱻵��ã���������Ϸ�������ú�ִ��һ���Գ�ʼ������
    protected virtual void Start()
    {
        // ����player��enemy����ͼ�����ײ(ȫ�ּ���)��playerͼ������6��enemyͼ������7����Ҫ�������������ײ��Ҫ�ٴε��øú�����������ͬ�㼶��Ȼ�����������Ϊfalse����
        Physics2D.IgnoreLayerCollision(6, 7);

        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponentInChildren<EntityFX>();
    }

    // ��Ϸ��ÿ֡�������
    protected virtual void Update()
    {
        
    }

    // �˺�������player��enemy���øú������������յ����˺�
    public virtual void Damage()
    {
        // ��һ��Э�̣���չʾ�����е�Ч��
        fx.StartCoroutine("FlashFX");

        Debug.Log(gameObject.name + " was danaged");
    }

    #region Velocity
    // ��player���ٶ���Ϊ0
    public void SetZeroVelocity() => rb.velocity = new Vector2(0, 0);

    // ������ҵ��ƶ��ٶȣ�����x�����y����
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
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
}
