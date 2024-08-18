using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* 所有预制件必须设置为enable状态，否则其实例化后不会执行后awake、start等方法，此处调用会报空指针异常等错误
* 预制件可以在samplescene中删除，但是在prefabs文件夹中的预制件必须是enable状态
*/

public class SwordSkillController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    // 飞出去的剑的碰撞体
    private CircleCollider2D cc;
    private Player player;
    // 剑是否需要旋转
    private bool canRotate = true;
    // 剑是否回收中
    private bool isReturning;

    // 被扔出去的剑击中后冻结的时间
    private float freezeTimeDuration;
    // 剑回收时的速度
    private float returnSpeed = 12;

    [Header("Pierce info")]
    [SerializeField] private float pierceAmount;

    [Header("Bounce info")]
    // 剑的反弹速度
    private float bounceSpeed;
    // 扔出去的sword是否在反弹中
    private bool isBouncing;
    // 剑反弹最多次数
    private int bounceAmount;
    // 在剑反弹范围内的enemy
    private List<Transform> enemyTarget;
    // 反弹enemy的索引
    private int targetIndex;

    [Header("Spin info")]
    // player和旋转的剑之间最大距离
    private float maxTravelDistance;
    // 旋转的时间
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpining;
    // 旋转剑的攻击定时器
    private float hitTimer;
    // 旋转剑的攻击频率，每次攻击间隔为hitCooldown
    private float hitCooldown;

    private float spinDirection;

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
        if (canRotate)
        {
            // 此时剑向右方向的速度为剑的移动速度，这样设置可以让剑在击中物体时呈现斜向下插入状态，而非平行于水平线向右状态
            transform.right = rb.velocity;
        }

        if (isReturning)
        {
            // 在二维空间中平滑地从一个点移动到另一个点, 参数： 当前位置；目标位置；在当前帧中，物体可以移动的最大距离
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchTheSword();
            }
        }

        BounceLogic();

        SpinLogic();
    }
    
    // 销毁扔出去的剑对象
    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed)
    {
        player = _player;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
        if(pierceAmount <=0 )
        {
            anim.SetBool("Rotation", true);
        }

        // 将一个值(rb.velocity.x)限制在两个指定的最小值（-1）和最大值（1）之间
        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        // 在指定的延迟时间后自动调用一个函数
        Invoke("DestroyMe", 7);
    }

    private void SpinLogic()
    {
        if (isSpining)
        {
            // 当sword和player的距离过大时
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpining();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                // 让旋转的sword一直向左或向右移动
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f *  Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpining = false;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (Collider2D collider in colliders)
                    {
                        if (collider.GetComponent<Enemy>() != null)
                        {
                            SwordSkillDamage(collider.GetComponent<Enemy>());
                        }
                    }
                }
            }
        }
    }

    // 停止sword继续向远处走，在此距离处开始旋转
    private void StopWhenSpining()
    {
        wasStopped = true;
        // 停止sword的位移，让sword在此处旋转
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        // 判断反弹的列表中是否为空，不为空且处于反弹中，则开始反弹
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }


    public void SetupBounce(bool _isBouncing, int _amountBounces, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountBounces;
        bounceSpeed = _bounceSpeed;

        // enemyTarget是private，所以需要手动创建，如果是public的则不需要这一句，unity会自动创建
        enemyTarget = new List<Transform>();
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupSpin(bool _isSpining, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpining = _isSpining;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    // 回收剑
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;   // 修复扔出去的剑必须得先触地然后才能返回的bug
        transform.parent = null;
        isReturning = true;
    }

    // 当一个游戏对象的 Collider2D（碰撞器）与另一个游戏对象的 Trigger2D（触发器）接触时，会触发这个函数
    // 注：要修改sword的CircleCollider2D的istrigger值为true，让sword的碰撞器可以触发该函数
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
        {
            return;
        }

        if(collision.GetComponent<Enemy>()!=null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        /*// ?.运算符，如果对象不为null，则正常访问其成员；如果对象为null，则不会尝试访问其成员，并且整个表达式的结果为null
        collision.GetComponent<Enemy>()?.Damage();*/
        SetupTargetForBounce(collision);

        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        enemy.Damage();
        enemy.StartCoroutine("FreezeTimerFor", freezeTimeDuration);
    }

    private void SetupTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)    // 击中enemy后才会反弹
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (Collider2D collider in colliders)
                {
                    // 在反弹的半径范围内对象是enemy的加入到列表中
                    if (collider.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(collider.transform);
                    }
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        // 旋转sword不会卡在任何对象中
        if(isSpining)
        { 
            StopWhenSpining();  // 扔出去旋转的剑碰到第一个enmey时停止，在此处旋转
            return; 
        }

        // 关闭剑的旋转
        canRotate = false;
        // 用于控制碰撞器是否参与物理碰撞和触发器事件, 为 false 时，碰撞器不会参与碰撞检测或触发器事件
        cc.enabled = false;
        // 指定 Rigidbody2D 是否受物理引擎控制, 设置为 true 时，该 Rigidbody2D 将不会受到物理引擎的力（如重力、碰撞力等）的影响，也不会参与物理碰撞的自动解决
        rb.isKinematic = true;
        // constraints 属性用于限制游戏对象在二维物理世界中的移动和旋转,
        // RigidbodyConstraints2D.FreezeAll该值会冻结（或约束）游戏对象在二维物理世界中的所有移动和旋转自由度
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
        {
            // 剑在反弹中不关闭旋转，不设置父对象
            return;
        }

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
