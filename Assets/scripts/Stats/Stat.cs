using System.Collections.Generic;
using UnityEngine;

// ����Ǳ�׼���ͣ����Զ����ࣩ�� Inspector ��������ʾ�ͱ༭
[System.Serializable]
public class Stat
{
    [SerializeField] private int baseValue;

    // ��baseValue����������ֵ�б���entityװ������������б�������������Ӧ�Ĺ�������ֵ
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