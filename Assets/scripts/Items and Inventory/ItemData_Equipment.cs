using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,   // �����
    Flask
}

// ��unity�ڲ�����һ��create������Ŀ��·����Data/Equipment��������Ĭ����ΪNew Item Data����Ĭ��������itemName��icon
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Major stats")]
    // �����������˺�
    public int strength;
    // ���ݣ��������ܣ����ӱ�������
    public int agility;
    // ����������ħ���˺���ħ������
    public int intelligence;
    // ������������Ѫ��
    public int vitality;

    [Header("Offensive stats")]
    // ������
    public int damage;
    // �������ʣ��������ݼӳ�
    public int critChange;
    // Ĭ��150%������ʱ���˺��ӳɱ���������ʱ��������Ȼ���100���ٳ����˺���Ϊ�����˺�ֵ
    public int critPower;

    [Header("Defensive stats")]
    // Ѫ��
    public int maxHealth;
    // װ��
    public int armor;
    // ����
    public int evasion;
    // ħ������
    public int magicResistance;

    [Header("Magic stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;

    /*
     * ͨ����ͬװ�������ԣ�����player�����Խ����޸�(���ӻ����)�������ǽ�����뵽player��modifier���б��У�
     * Ȼ���ڼ���player��ĳһ����ֵʱ�������modifier�б��е�ֵ
     */
    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChange.AddModifier(critChange);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(maxHealth);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);
    }

    public void RemoveModifiers() 
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChange.RemoveModifier(critChange);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(maxHealth);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);
    }
}
