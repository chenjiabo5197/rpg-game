using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cc => GetComponent<CircleCollider2D>();

    private float crystalExistTimer;

    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 5;

    private Transform closestEnemy = null;
    [SerializeField] private LayerMask whatIsEnemy;

    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestEnemy)
    {
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestEnemy = _closestEnemy;
    }

    public void ChooseRandomEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, SkillManager.instance.blackhole.GetBlackholeRadius(), whatIsEnemy);

        if (colliders.Length > 0)
        {
            closestEnemy = colliders[Random.Range(0, colliders.Length)].transform;
        }
    }

    public void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if(canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);

            if(Vector2.Distance(transform.position, closestEnemy.position) < 1)
            {
                FinishCrystal();
                // 爆炸后不再移动crystal
                canMove = false;
            }
        }

        if(canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }
    }

    private void AnimationExplodeEvent()
    {
        // 获取此时在攻击球范围内所有物体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cc.radius );

        foreach (var hit in colliders)
        {
            // 如果列表中物体是enemy，则调用enemy的damage函数，表示enemy收到伤害
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().DamageEffect();
            }
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    public void SelfDestroy() => Destroy(gameObject);
}
