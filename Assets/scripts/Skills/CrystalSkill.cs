using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    // crystal����ʱ�䣬������ʱ��û�������ͷţ���crystal�����Զ��ͷ�
    [SerializeField] private float crystalDuration;
    private GameObject currentCrystal;

    [Header("Crystal mirage")]
    // �Ƿ������crystalλ������һ��clone�����������������ߺ���finish���̣��������ɽ���finish����
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Explosive Info")]
    // ˮ���Ƿ�ɱ�ը
    [SerializeField] bool canExplode;

    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
    // �Ƿ����Multi stacking crystal
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    // ��ȴʱ��
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    // ���е�crystal�б�
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    /*
     * ����crystalDurationʱ����û������ʹ��crystal���ܣ������FinishCrystal������
     *                          ������ʹ��crystal���ܣ���player��crystalλ�û�����Ȼ�����FinishCrystal������
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
                // ���crystal���ƶ�����player�޷���crystal����λ��
                return;
            }

            // ����ʹ�ù�crystal���ܣ��򻥻�player��currentCrystal��λ��
            Vector3 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if(cloneInsteadOfCrystal)
            {
                // ������������player��currentCrystal��λ�û��������Դ˴������ɵ�clone����ʵ����player��λ�ã�player�䵽currentCrystal��λ�ã�currentCrystal������
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                // ����ȡ��CrystalSkillController������ִ��CrystalSkillController�����FinishCrystal����������Ϊnull�����쳣
                currentCrystal.GetComponent<CrystalSkillController>()?.FinishCrystal();
            }
        }
    }

    public void CreateCrystal()
    {
        // ʵ����ˮ�����󣬶�����anmintor��controller��component
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
                // �ս�����ж�ʱ���б���crystal��������amountOfStacks��ֻ��������Ż��ͷ�crystal��Ȼ��ú�������useTimeWindowʱ��󲹳�ȱ�ٵ�crystal��
                // ʹ�õ��ڵ��ж�������crystal�о���Ϊ�˱���Ƶ�������б��crystal
                if (crystalLeft.Count == amountOfStacks)
                {
                    Invoke("ResetAbility", useTimeWindow);
                }

                cooldown = 0;  // ���б�����crystalʱ����ʱ��ȴʱ��һֱΪ0�����ڲ���ȴ״̬
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

    // ���crystalLeft�б�������ΪamountOfStacks
    private void RefillCrystal()
    {

        for (int i = crystalLeft.Count; i < amountOfStacks; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if(cooldownTimer > 0)  // �ڼ�����ȴʱ���ڲ�����crystal
        {
            return;
        }
        // ���ڼ�����ȴʱ���ڣ�������crystal�б�������ȴʱ��
        cooldownTimer = multiStackCooldown;
        RefillCrystal();
    }
}
