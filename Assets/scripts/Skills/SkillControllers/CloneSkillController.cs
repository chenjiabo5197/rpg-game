using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    // 渲染器
    private SpriteRenderer sr;
    private Animator anim;
    // clone体变透明的速度
    [SerializeField] private float colorLossingSpeed;
    // 定时器
    private float cloneTimer;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;
    private Transform closestEnemy;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            // 设置其alpha值，让其逐渐变透明
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLossingSpeed));

            if (sr.color.a < 0)
            {
                // 当clone体透明到看不到时，删除该clone对象
                Destroy(gameObject);
            }
        }
    }

    // 设置clone体的位置
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset)
    {
        if(_canAttack)
        {
            // 随机播放攻击1-3任意动画
            anim.SetInteger("AttackNumber", Random.Range(1, 3));
        }

        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;

        FaceClosestTarget();
    }

    private Player player => GetComponentInParent<Player>();

    // 为了停止攻击，一般在攻击动画的后一帧调用，攻击结束后，设置定时器值小于0，使其进入消失流程
    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        // 获取此时在攻击球范围内所有物体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            // 如果列表中物体是enemy，则调用enemy的damage函数，表示enemy收到伤害
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }

    // 使clone对象面向最近的攻击对象
    private void FaceClosestTarget()
    {
        // 获取此时在25范围内所有物体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, 25);
        // 正无穷大
        float closestDistance = Mathf.Infinity;

        foreach (var hit in colliders)
        {
            // 循环获取离clone体最近的enemy对象
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if(distanceToEnemy < closestDistance)
                {
                    closestEnemy = hit.transform;
                    closestDistance = distanceToEnemy;
                }
            }
        }

        if (closestEnemy != null)
        {
            // 如果此时clone体在离最近的enemy的右侧的话，则clone旋转180，面向此enemy
            if(transform.position.x > closestEnemy.transform.position.x)
            {
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
