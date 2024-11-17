using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,   // 护身符
    Flask
}

// 在unity内部新增一个create的新项目，路径是Data/Equipment，创建后默认名为New Item Data，其默认属性有itemName和icon
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Major stats")]
    // 力量，增加伤害
    public int strength;
    // 敏捷，增加闪避，增加暴击概率
    public int agility;
    // 智力，增加魔法伤害和魔法抗性
    public int intelligence;
    // 生命力，增加血量
    public int vitality;

    [Header("Offensive stats")]
    // 攻击力
    public int damage;
    // 暴击概率，会有敏捷加成
    public int critChange;
    // 默认150%，暴击时的伤害加成比例，计算时加上力量然后除100，再乘总伤害即为暴击伤害值
    public int critPower;

    [Header("Defensive stats")]
    // 血量
    public int maxHealth;
    // 装甲
    public int armor;
    // 闪避
    public int evasion;
    // 魔法抗性
    public int magicResistance;

    [Header("Magic stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;

    /*
     * 通过不同装备的属性，来对player的属性进行修改(增加或减少)，增加是将其加入到player的modifier的列表中，
     * 然后在计算player的某一属性值时，会带上modifier列表中的值
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
