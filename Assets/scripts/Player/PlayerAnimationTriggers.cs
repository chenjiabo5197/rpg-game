using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    // Ϊ�˵���player�е�AnimationTrigger��������triggerCalled������Ϊtrue��ֹͣ������һ���ڹ��������ĺ�һ֡����
    private void AnimationTrigger()
    {
        // һ����animator�еĲ�����ã�����player��animator�ĸ��ڵ㣬����Ҫ�ȴӸ����л�ȡ��player��Ȼ���ٵ���player������triggerCalled��Ϊtrue����playerstate��ֹͣ��������
        player.AnimationTrigger();
    }
}
