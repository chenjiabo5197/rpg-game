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

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    // ��һ��Э�̣��Ƚ�material��Ϊ���ʱ��material���ȴ�һ��ʱ����ٽ�����Ϊԭʼ��material
    private IEnumerator FlashFX()
    {
        sr.material = hitMat;

        yield return new WaitForSeconds(flashDuration);

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
    private void CancelRedBlink()
    {
        // ȡ������ͨ�� Invoke �� InvokeRepeating ���õĵ���
        CancelInvoke();

        sr.color = Color.white;
    }
}
