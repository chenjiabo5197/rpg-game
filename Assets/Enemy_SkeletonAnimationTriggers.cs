using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkeletonAnimationTriggers : MonoBehaviour
{
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    // Ϊ�˵���EnemySkeleton�е�AnimationTrigger��������triggerCalled������Ϊtrue��ֹͣ������һ���ڹ��������ĺ�һ֡����
    private void AnimationTrigger()
    {
        // һ����animator�еĲ�����ã�����EnemySkeleton��animator�ĸ��ڵ㣬����Ҫ�ȴӸ����л�ȡ��EnemySkeleton��Ȼ���ٵ���EnemySkeleton������triggerCalled��Ϊtrue
        enemy.AnimationTrigger();
    }

}
