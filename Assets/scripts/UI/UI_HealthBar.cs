using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    // 访问数据
    private CharacterStats myStats;
    // 血条的旋转对象，用于控制血条旋转
    private RectTransform myTransform;
    private Slider slider;

    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();

        // 在entity的onFlipped事件中添加FlipUI事件
        entity.onFlipped += FlipUI;
        // 血量修改事件触发修改血条
        myStats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    // 为了减少压力，不在每帧中更新血量，采用事件触发方式
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

    // 取消订阅事件
    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }
}
