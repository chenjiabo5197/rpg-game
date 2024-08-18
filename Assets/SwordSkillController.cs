using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    // 剑回收时的速度
    [SerializeField] private float returnSpeed = 12;
    private Animator anim;
    private Rigidbody2D rb;
    // 飞出去的剑的碰撞体
    private CircleCollider2D cc;
    private Player player;
    // 剑是否需要旋转
    private bool canRotate = true;
    // 剑是否回收中
    private bool isReturning;

    private void Awake()
    {
        // 注意，此处放到start里会出现空指针异常
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {

    }

    public void Update()
    {
        if(canRotate)
        {
            transform.right = rb.velocity;
        }

        if(isReturning)
        {
            // 在二维空间中平滑地从一个点移动到另一个点, 参数： 当前位置；目标位置；在当前帧中，物体可以移动的最大距离
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1 )
            {
                player.ClearTheSword();
            }
        }
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player)
    {
        player = _player;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        anim.SetBool("Rotation", true);
    }

    // 回收剑
    public void ReturnSword()
    {
        rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    // 当一个游戏对象的 Collider2D（碰撞器）与另一个游戏对象的 Trigger2D（触发器）接触时，会触发这个函数
    // 注：要修改sword的CircleCollider2D的istrigger值为true，让sword的碰撞器可以触发该函数
    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetBool("Rotation", false);
        // 关闭剑的旋转
        canRotate = false;
        // 用于控制碰撞器是否参与物理碰撞和触发器事件, 为 false 时，碰撞器不会参与碰撞检测或触发器事件
        cc.enabled = false;
        // 指定 Rigidbody2D 是否受物理引擎控制, 设置为 true 时，该 Rigidbody2D 将不会受到物理引擎的力（如重力、碰撞力等）的影响，也不会参与物理碰撞的自动解决
        rb.isKinematic = true;
        // constraints 属性用于限制游戏对象在二维物理世界中的移动和旋转,
        // RigidbodyConstraints2D.FreezeAll该值会冻结（或约束）游戏对象在二维物理世界中的所有移动和旋转自由度
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;
    }
}
