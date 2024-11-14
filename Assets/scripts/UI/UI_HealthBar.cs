using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    // ��������
    private CharacterStats myStats;
    // Ѫ������ת�������ڿ���Ѫ����ת
    private RectTransform myTransform;
    private Slider slider;

    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();

        // ��entity��onFlipped�¼������FlipUI�¼�
        entity.onFlipped += FlipUI;
        // Ѫ���޸��¼������޸�Ѫ��
        myStats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    // Ϊ�˼���ѹ��������ÿ֡�и���Ѫ���������¼�������ʽ
    /*private void Update()
    {
        UpdateHealthUI();
    }*/

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }

    private void FlipUI() => myTransform.Rotate(0, 180, 0);

    // ȡ�������¼�
    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }
}
