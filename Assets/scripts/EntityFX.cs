using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    // 被击中的效果展示时间
    [SerializeField] private float flashDuration;
    // 被击中时的展示效果
    [SerializeField] private Material hitMat;
    // 原始的展示效果
    private Material originalMat;

    [Header("Ailment colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
        {
            sr.color = Color.clear;
        }
        else
        {
            sr.color = Color.white;
        }
    }

    // 起一个协程，先将material置为打击时的material，等待一定时间后，再将其置为原始的material
    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        // 打击效果结束后，从上一个颜色继续开始
        sr.color = currentColor;
        sr.material = originalMat;
    }

    // 红白闪烁的效果
    private void RedColorBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }
    }

    // 取消上面闪烁函数的效果
    private void CancelColorChange()
    {
        // 取消所有通过 Invoke 或 InvokeRepeating 设置的调用
        CancelInvoke();

        sr.color = Color.white;
    }

    public void IgniteFxFor(float _seconds)
    {
        // 从0s开始循环调用IgniteColorFx函数，每次调用间隔0.3s
        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ChillFxFor(float _seconds)
    {
        // 从0s开始循环调用ChillColorFx函数，每次调用间隔0.3s
        InvokeRepeating("ChillColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ShockFxFor(float _seconds)
    {
        // 从0s开始循环调用ShockColorFx函数，每次调用间隔0.3s
        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgniteColorFx()
    {
        // 在igniteColor数组的两个颜色之间切换
        if (sr.color != igniteColor[0])
        {
            sr.color = igniteColor[0];
        }
        else
        {
            sr.color = igniteColor[1];
        }
    }

    private void ChillColorFx()
    {
        // 在chillColor数组的两个颜色之间切换
        if (sr.color != chillColor[0])
        {
            sr.color = chillColor[0];
        }
        else
        {
            sr.color = chillColor[1];
        }
    }

    private void ShockColorFx()
    {
        // 在shockColor数组的两个颜色之间切换
        if (sr.color != shockColor[0])
        {
            sr.color = shockColor[0];
        }
        else
        {
            sr.color = shockColor[1];
        }
    }
}
