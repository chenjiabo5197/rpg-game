using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    // �����е�Ч��չʾʱ��
    [SerializeField] private float flashDuration;
    // ������ʱ��չʾЧ��
    [SerializeField] private Material hitMat;
    // ԭʼ��չʾЧ��
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

    // ��һ��Э�̣��Ƚ�material��Ϊ���ʱ��material���ȴ�һ��ʱ����ٽ�����Ϊԭʼ��material
    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        // ���Ч�������󣬴���һ����ɫ������ʼ
        sr.color = currentColor;
        sr.material = originalMat;
    }

    // �����˸��Ч��
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

    // ȡ��������˸������Ч��
    private void CancelColorChange()
    {
        // ȡ������ͨ�� Invoke �� InvokeRepeating ���õĵ���
        CancelInvoke();

        sr.color = Color.white;
    }

    public void IgniteFxFor(float _seconds)
    {
        // ��0s��ʼѭ������IgniteColorFx������ÿ�ε��ü��0.3s
        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ChillFxFor(float _seconds)
    {
        // ��0s��ʼѭ������ChillColorFx������ÿ�ε��ü��0.3s
        InvokeRepeating("ChillColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ShockFxFor(float _seconds)
    {
        // ��0s��ʼѭ������ShockColorFx������ÿ�ε��ü��0.3s
        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgniteColorFx()
    {
        // ��igniteColor�����������ɫ֮���л�
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
        // ��chillColor�����������ɫ֮���л�
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
        // ��shockColor�����������ɫ֮���л�
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
