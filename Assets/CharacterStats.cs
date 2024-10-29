using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")]
    // 力量，增加伤害
    public Stat strength;
    // 敏捷，增加闪避，增加暴击概率
    public Stat agility;
    // 智力，增加魔法伤害和魔法抗性
    public Stat intelligence;
    // 生命力，增加血量
    public Stat vitality;

    [Header("Offensive stats")]
    // 攻击力
    public Stat damage;
    // 暴击概率，会有敏捷加成
    public Stat critChange;
    // 默认150%，暴击时的伤害加成比例，计算时加上力量然后除100，再乘总伤害即为暴击伤害值
    public Stat critPower;

    [Header("Defensive stats")]
    // 血量
    public Stat maxHealth;
    // 装甲
    public Stat armor;
    // 闪避
    public Stat evasion;
    // 魔法抗性
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    // 对应上面的火、冰、雷电伤，判断是否处于被魔法击中的状态，不同状态有不同的增益和衰减
    // 是否被点燃，每秒收到伤害
    public bool isIgnited;
    // 是否被冰冻，减少20%护甲
    public bool isChilled;
    // 是否被眩晕，减少20闪避
    public bool isShocked;

    [SerializeField] private float ailmentsDuration = 4;
    // 点燃状态的定时器，点燃对象可以维持点燃状态的时间，时间小于0后，从点燃状态出来
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;
    
    // 点燃后每次造成伤害之间的间隔
    private float igniteDamageCooldown = .3f;
    // 点燃受伤的定时器，在每次update时减少，小于0后完成一次受伤
    private float igniteDamageTimer;
    // 点燃后每次造成的伤害
    private int igniteDamage;
    // 雷击对象的预制件
    [SerializeField] private GameObject shockStrikePrefab;
    // 雷击造成的伤害
    private int shockDamage;

    public int currentHealth;

    // 血量改变事件，发生该事件后，修改entity的血条
    public System.Action onHealthChanged;

    public virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
        {
            // 从点燃状态退出
            isIgnited = false;
        }
        if (chilledTimer < 0)
        {
            isChilled = false;
        }
        if (shockedTimer < 0)
        {
            isShocked = false;
        }
        if (igniteDamageTimer < 0 && isIgnited)
        {
            // 完成一次点燃伤害
            igniteDamageTimer = igniteDamageCooldown;
            Debug.Log("igniteDamageTimer");
            DecreaseHealthBy(igniteDamage);
            if (currentHealth < 0)
            {
                Dead();
            }
        }
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }
        int totalDamage = damage.GetValue() + strength.GetValue();

        if(CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }    

        totalDamage = CheckTargetArnor(_targetStats, totalDamage);
        // 物理伤害
        _targetStats.TakeDamage(totalDamage);
        // 魔法伤害
        DoMagiclDamage(_targetStats);
    }

    public virtual void DoMagiclDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        // 每1点智力增加1点魔法伤害
        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);
        //_targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
        {
            return;
        }

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .5f && _fireDamage > 0)
            {
                canApplyChill = true;
                _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if(canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        }
        if(canApplyShock)
        {
            _targetStats.SetupShockDamage(Mathf.RoundToInt(_lightingDamage * .1f));
        }

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private static int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        // 每1点智力增加3点魔法伤害
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        // 判断对象当前是否处于伤害中，处于则返回
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;


        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;

            fx.IgniteFxFor(ailmentsDuration);
        }
        if (_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = ailmentsDuration;
            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            fx.ChillFxFor(ailmentsDuration);
        }
        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                // 防止enemy在攻击player时触发雷击中enemy
                if (GetComponent<Player>() != null)
                {
                    return;
                }

                HitNearestTargetWithShockStrike();
            }
        }
    }

    public void ApplyShock(bool _shock)
    {
        if(isShocked)
        {
            return;
        }

        isShocked = _shock;
        shockedTimer = ailmentsDuration;
        fx.ShockFxFor(ailmentsDuration);
    }

    private void HitNearestTargetWithShockStrike()
    {
        // 找到距离最近的enemy对象
        Transform closestEnemy = null;
        // 获取此时在25范围内所有物体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        // 正无穷大
        float closestDistance = Mathf.Infinity;

        foreach (var hit in colliders)
        {
            // 循环获取离clone体最近的enemy对象
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestEnemy = hit.transform;
                    closestDistance = distanceToEnemy;
                }
            }
            if (closestEnemy == null)
            {
                closestEnemy = transform;
            }
        }
        // 实例化雷击对象
        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            // 设置雷击参数值
            newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    public void SetupShockDamage(int _damage) => shockDamage = _damage;

    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);

        if (currentHealth < 0)
        {
            Dead();
        }
    }

    // 血量减少函数
    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;

        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
    }

    protected virtual void Dead()
    {

    }

    private static int CheckTargetArnor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
        {
            // 若攻击对象处于冰冻状态，则其护甲值减少20%
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        }
        else
        {
            totalDamage -= _targetStats.armor.GetValue();
        }
        // 对伤害设定最小和最大值，防止伤害值小于0时，击中敌人会给敌人加血
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        // 计算总闪避的概率
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
        {
            // 若处于眩晕状态，则攻击对象增加20的闪避几率
            totalEvasion += 20;
        }

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    private bool CanCrit()
    {
        int totalCriticalChance = critChange.GetValue() + agility.GetValue();

        if(Random.Range(0, 100) < totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    // 计算暴击伤害，暴击伤害最少为默认伤害值得1.5倍
    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        float critDamage = _damage * totalCritPower;
        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        // 1点生命力增加5点血
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
}
