using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    // ���ܵ���ȴʱ��
    [SerializeField] protected float cooldown;
    // ������ȴʱ��ļ�ʱ��
    protected float cooldownTimer;

    protected Player player;

    private void Awake()
    {
        
    }

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            // ������ȴʱ���ڣ�������ȴʱ���ʱ��
            cooldownTimer = cooldown;
            return true;
        }
        Debug.Log("skill is on cooldown");
        return false;
    }

    public virtual void UseSkill()
    {
        Debug.Log("use skill");
    }

    // ��ȡ����_checkTransform�����enemy
    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        Transform closestEnemy = null;
        // ��ȡ��ʱ��25��Χ����������
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);
        // �������
        float closestDistance = Mathf.Infinity;

        foreach (var hit in colliders)
        {
            // ѭ����ȡ��clone�������enemy����
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestEnemy = hit.transform;
                    closestDistance = distanceToEnemy;
                }
            }
        }
        return closestEnemy;
    }
}
