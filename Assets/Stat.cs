using System.Collections.Generic;
using UnityEngine;

// 允许非标准类型（如自定义类）在 Inspector 窗口中显示和编辑
[System.Serializable]
public class Stat
{
    [SerializeField] private int baseValue;

    // 对baseValue有增幅的数值列表，若entity装备武器，则该列表中增加武器对应的攻击力数值
    public List<int> modifiers;

    public int GetValue()
    {
        int finalValue = baseValue;

        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }

        return finalValue;
    }

    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }

    public void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier);
    }

    public void RemoveModifier(int _modifier)
    {
        modifiers.Remove(_modifier);
    }
}