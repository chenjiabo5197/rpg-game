using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    // crystal持续时间，超过此时间没有主动释放，则crystal技能自动释放
    [SerializeField] private float crystalDuration;
    private GameObject currentCrystal;

    [Header("Crystal mirage")]
    // 是否可以在crystal位置生成一个clone对象，若可以生成则不走后续finish流程，不能生成进入finish流程
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Explosive Info")]
    // 水晶是否可爆炸
    [SerializeField] bool canExplode;

    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
    // 是否可用Multi stacking crystal
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    // 冷却时间
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    // 现有的crystal列表
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    /*
     * 若在crystalDuration时间内没有主动使用crystal技能，则进入FinishCrystal函数中
     *                          若主动使用crystal技能，则player与crystal位置互换，然后进入FinishCrystal函数中
     */

    public override void UseSkill()
    {
        base.UseSkill();

        if(CanUseMultiCrystal())
        {
            return;
        }

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if(canMoveToEnemy)
            {
                // 如果crystal在移动，则player无法与crystal交换位置
                return;
            }

            // 主动使用过crystal技能，则互换player与currentCrystal的位置
            Vector3 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if(cloneInsteadOfCrystal)
            {
                // 上面代码进行了player与currentCrystal的位置互换，所以此处新生成的clone对象实际在player的位置，player变到currentCrystal的位置，currentCrystal回收了
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                // 若获取到CrystalSkillController对象，则执行CrystalSkillController对象的FinishCrystal防范，否则为null且无异常
                currentCrystal.GetComponent<CrystalSkillController>()?.FinishCrystal();
            }
        }
    }

    public void CreateCrystal()
    {
        // 实例化水晶对象，对象有anmintor和controller等component
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        CrystalSkillController controller = currentCrystal.GetComponent<CrystalSkillController>();
        controller.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform), player);
    }

    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<CrystalSkillController>().ChooseRandomEnemy();

    private bool CanUseMultiCrystal()
    {
        if(canUseMultiStacks)
        {
            if (crystalLeft.Count > 0)
            {
                // 刚进入此判断时，列表中crystal的数量是amountOfStacks，只有在下面才会释放crystal，然后该函数会在useTimeWindow时间后补充缺少的crystal，
                // 使用等于的判断来补充crystal感觉是为了避免频繁补充列表的crystal
                if (crystalLeft.Count == amountOfStacks)
                {
                    Invoke("ResetAbility", useTimeWindow);
                }

                cooldown = 0;  // 当列表中有crystal时，此时冷却时间一直为0，处于不冷却状态
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);
                newCrystal.GetComponent<CrystalSkillController>().
                    SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform), player);

                if (crystalLeft.Count <= 0)
                { 
                    cooldown = multiStackCooldown;
                    RefillCrystal();
                }
                return true;
            }
        }
        return false;
    }

    // 填充crystalLeft列表，填充个数为amountOfStacks
    private void RefillCrystal()
    {

        for (int i = crystalLeft.Count; i < amountOfStacks; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if(cooldownTimer > 0)  // 在技能冷却时间内不补充crystal
        {
            return;
        }
        // 不在技能冷却时间内，补充满crystal列表，重置冷却时间
        cooldownTimer = multiStackCooldown;
        RefillCrystal();
    }
}
