using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    // ��Ⱦ��
    private SpriteRenderer sr;
    private Animator anim;
    // clone���͸�����ٶ�
    [SerializeField] private float colorLossingSpeed;
    // ��ʱ��
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
            // ������alphaֵ�������𽥱�͸��
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLossingSpeed));

            if (sr.color.a < 0)
            {
                // ��clone��͸����������ʱ��ɾ����clone����
                Destroy(gameObject);
            }
        }
    }

    // ����clone���λ��
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack)
    {
        if(_canAttack)
        {
            // ������Ź���1-3���⶯��
            anim.SetInteger("AttackNumber", Random.Range(1, 3));
        }

        transform.position = _newTransform.position;
        cloneTimer = _cloneDuration;

        FaceClosestTarget();
    }

    private Player player => GetComponentInParent<Player>();

    // Ϊ��ֹͣ������һ���ڹ��������ĺ�һ֡���ã��������������ö�ʱ��ֵС��0��ʹ�������ʧ����
    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        // ��ȡ��ʱ�ڹ�����Χ����������
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            // ����б���������enemy�������enemy��damage��������ʾenemy�յ��˺�
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }

    // ʹclone������������Ĺ�������
    private void FaceClosestTarget()
    {
        // ��ȡ��ʱ��25��Χ����������
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, 25);
        // �������
        float closestDistance = Mathf.Infinity;

        foreach (var hit in colliders)
        {
            // ѭ����ȡ��clone�������enemy����
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
            // �����ʱclone�����������enemy���Ҳ�Ļ�����clone��ת180�������enemy
            if(transform.position.x > closestEnemy.transform.position.x)
            {
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
