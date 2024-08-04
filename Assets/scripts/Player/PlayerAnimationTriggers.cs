using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    // 为了调用player中的AnimationTrigger函数，将triggerCalled变量置为true，停止攻击，一般在攻击动画的后一帧调用
    private void AnimationTrigger()
    {
        // 一般是animator中的插件调用，但是player是animator的父节点，所以要先从父类中获取到player，然后再调用player函数，triggerCalled置为true，在playerstate中停止攻击动画
        player.AnimationTrigger();
    }
}
