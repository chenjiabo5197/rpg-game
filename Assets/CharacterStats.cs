using System.Xml.Serialization;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    // �����������˺�
    public Stat strength;
    // ���ݣ��������ܣ����ӱ�������
    public Stat agility;
    // ����������ħ���˺���ħ������
    public Stat intelligence;
    // ������������Ѫ��
    public Stat vitality;

    [Header("Offensive stats")]
    // ������
    public Stat damage;
    // �������ʣ��������ݼӳ�
    public Stat critChange;
    // Ĭ��150%������ʱ���˺��ӳɱ���������ʱ��������Ȼ���100���ٳ����˺���Ϊ�����˺�ֵ
    public Stat critPower;

    [Header("Defensive stats")]
    // Ѫ��
    public Stat maxHealth;
    // װ��
    public Stat armor;
    // ����
    public Stat evasion;
    // ħ������
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    // ��Ӧ����Ļ𡢱����׵��ˣ��ж��Ƿ��ڱ�ħ�����е�״̬����ͬ״̬�в�ͬ�������˥��
    // �Ƿ񱻵�ȼ��ÿ���յ��˺�
    public bool isIgnited;
    // �Ƿ񱻱���������20%����
    public bool isChilled;
    // �Ƿ�ѣ�Σ�����20����
    public bool isShocked;

    // ��ȼ״̬�Ķ�ʱ������ȼ�������ά�ֵ�ȼ״̬��ʱ�䣬ʱ��С��0�󣬴ӵ�ȼ״̬����
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;
    // ��ȼ��ÿ������˺�֮��ļ��
    private float igniteDamageCooldown = .3f;
    // ��ȼ���˵Ķ�ʱ������ÿ��updateʱ���٣�С��0�����һ������
    private float igniteDamageTimer;
    // ��ȼ��ÿ����ɵ��˺�
    private int igniteDamage;

    [SerializeField] private int currentHealth;

    public virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = maxHealth.GetValue();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
        {
            // �ӵ�ȼ״̬�˳�
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
            // ���һ�ε�ȼ�˺�
            igniteDamageTimer = igniteDamageCooldown;
            Debug.Log("igniteDamageTimer");
            currentHealth -= igniteDamage;
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
        // _targetStats.TakeDamage(totalDamage);
        DoMagiclDamage(_targetStats);
    }

    public virtual void DoMagiclDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        // ÿ1����������1��ħ���˺�
        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);
        _targetStats.TakeDamage(totalMagicalDamage);

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

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private static int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        // ÿ1����������3��ħ���˺�
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        // �ж϶���ǰ�Ƿ����˺��У������򷵻�
        if (isIgnited || isChilled || isShocked)
        {
            return;
        }
        if (_ignite)
        {
            isIgnited = _ignite;
            ignitedTimer = 2;
        }
        if (_chill)
        {
            isChilled = _chill;
            chilledTimer = 2;
        }
        if (_shock)
        {
            isShocked = _shock;
            shockedTimer = 2;
        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;

    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;

        if (currentHealth < 0)
        {
            Dead();
        }
    }

    protected virtual void Dead()
    {

    }

    private static int CheckTargetArnor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
        {
            // �����������ڱ���״̬�����令��ֵ����20%
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        }
        else
        {
            totalDamage -= _targetStats.armor.GetValue();
        }
        // ���˺��趨��С�����ֵ����ֹ�˺�ֵС��0ʱ�����е��˻�����˼�Ѫ
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        // ���������ܵĸ���
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
        {
            // ������ѣ��״̬���򹥻���������20�����ܼ���
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

    // ���㱩���˺��������˺�����ΪĬ���˺�ֵ��1.5��
    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        float critDamage = _damage * totalCritPower;
        return Mathf.RoundToInt(critDamage);
    }
}
