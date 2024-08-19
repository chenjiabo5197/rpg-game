using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    // hotkey list，会自动向出现在blackhole中的enemy对象分配一个唯一的hoykey
    [SerializeField] private List<KeyCode> keyCodeList;

    [Header("Blackhole")]
    // blackhole的最大值
    private float maxSize;
    private float growSpeed;
    // blackhole 收缩的速度
    private float shrinkSpeed;
    private bool canGrow = true;
    private bool canShrink;

    [Header("Clone Attack")]
    private bool canCreateHotKey = true;
    private bool cloneAttackReleased;
    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;

    // 在blackhole中的enmey的transform的list，通过按enemy头顶的按键将其加入到该列表中
    private List<Transform> targets = new List<Transform>();
    // 创建的hotKeyPrefab预制件对象list，可以通过此list将创建的预制件删除
    private List<GameObject> createdHotKey = new List<GameObject>();

    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
    }

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            // Vector2.Lerp在两个二维向量之间执行线性插值,参数：起始向量；目标向量；插值参数。
            // t 的值在 0 到 1 之间。当 t=0 时，返回 起始向量；当 t=1 时，返回 目标向量。当 t 的值在 0 和 1 之间时，函数会返回 起始向量 和 目标向量 之间的一个点，
            // 这个点基于 t 的值线性地位于 起始向量 和 目标向量 之间。返回一个位于 起始向量 和 目标向量 之间的 Vector2，具体位置取决于 t 的值。
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            // 当blackhole的大小缩小到小于0时，销毁该对象
            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ReleaseCloneAttack()
    {
        DestroyHotKeys();
        cloneAttackReleased = true;
        canCreateHotKey = false;
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased)
        {
            cloneAttackTimer = cloneAttackCooldown;
            int randomIndex = Random.Range(0, targets.Count);
            if (targets.Count == 0)
            {
                Debug.LogError("targets list is empty!");
            }

            float xOffset;
            if (Random.Range(0, 100) > 50)
            {
                xOffset = 2;
            }
            else
            {
                xOffset = -2;
            }
            // 增加偏移量让clone出现的位置与enemy的位置有差异，二者不重叠
            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                canShrink = true;  // 攻击释放完后，blackhole自动进入收缩状态
                cloneAttackReleased = false;
            }
        }
    }

    private void DestroyHotKeys()
    {
        if(createdHotKey.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }

    // 当任何带有 Collider2D 组件的 GameObject（作为触发器）与另一个带有 Rigidbody2D 或 Collider2D（且至少一个组件的 Is Trigger 属性被设置为 true）的
    // GameObject 发生接触并进入其触发器区域时，OnTriggerEnter2D 方法会被调用。此例中blackhole的istrigger被设置为true
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);
        }
    }

    // 当一个游戏对象（必须带有Collider2D组件，并且该组件的Is Trigger属性被勾选）离开另一个游戏对象的触发器区域时，如果后者也带有Collider2D组件并且同样被设置为触发器，
    // 那么离开的游戏对象上的脚本中的OnTriggerExit2D函数将被调用
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent <Enemy>().FreezeTime(false);
        }
    }

    private void CreateHotKey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
        {
            Debug.LogWarning("keyCodeList is empty");
            return;
        }

        // clone体开始释放攻击后，再次进入blackhole的enemy不会导致创建hotkey，从而在删除hotkey时不会有遗留
        if(!canCreateHotKey)
        {
            return;
        }

        // 创建的hotkey实例在当前位置的正上方+2处
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);
        // 从keyCodeList中随机选取一个hotkey
        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        // 从keyCodeList删除该key
        keyCodeList.Remove(choosenKey);
        // 获取hotkey预制件上的controller
        BlackholeHotKeyController blackholeHotKeyController = newHotKey.GetComponent<BlackholeHotKeyController>();
        // 设置该hotkey
        blackholeHotKeyController.SetupHotKey(choosenKey, collision.transform, this);
    }

    // 将传入的transform加入到目标列表中
    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
