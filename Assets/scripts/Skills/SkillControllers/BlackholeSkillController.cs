using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    // hotkey list�����Զ��������blackhole�е�enemy�������һ��Ψһ��hoykey
    [SerializeField] private List<KeyCode> keyCodeList;

    [Header("Blackhole")]
    // blackhole�����ֵ
    private float maxSize;
    private float growSpeed;
    // blackhole �������ٶ�
    private float shrinkSpeed;
    private bool canGrow = true;
    private bool canShrink;

    [Header("Clone Attack")]
    private bool canCreateHotKey = true;
    private bool cloneAttackReleased;
    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;

    // ��blackhole�е�enmey��transform��list��ͨ����enemyͷ���İ���������뵽���б���
    private List<Transform> targets = new List<Transform>();
    // ������hotKeyPrefabԤ�Ƽ�����list������ͨ����list��������Ԥ�Ƽ�ɾ��
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
            // Vector2.Lerp��������ά����֮��ִ�����Բ�ֵ,��������ʼ������Ŀ����������ֵ������
            // t ��ֵ�� 0 �� 1 ֮�䡣�� t=0 ʱ������ ��ʼ�������� t=1 ʱ������ Ŀ���������� t ��ֵ�� 0 �� 1 ֮��ʱ�������᷵�� ��ʼ���� �� Ŀ������ ֮���һ���㣬
            // �������� t ��ֵ���Ե�λ�� ��ʼ���� �� Ŀ������ ֮�䡣����һ��λ�� ��ʼ���� �� Ŀ������ ֮��� Vector2������λ��ȡ���� t ��ֵ��
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            // ��blackhole�Ĵ�С��С��С��0ʱ�����ٸö���
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
            // ����ƫ������clone���ֵ�λ����enemy��λ���в��죬���߲��ص�
            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                canShrink = true;  // �����ͷ����blackhole�Զ���������״̬
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

    // ���κδ��� Collider2D ����� GameObject����Ϊ������������һ������ Rigidbody2D �� Collider2D��������һ������� Is Trigger ���Ա�����Ϊ true����
    // GameObject �����Ӵ��������䴥��������ʱ��OnTriggerEnter2D �����ᱻ���á�������blackhole��istrigger������Ϊtrue
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);
        }
    }

    // ��һ����Ϸ���󣨱������Collider2D��������Ҹ������Is Trigger���Ա���ѡ���뿪��һ����Ϸ����Ĵ���������ʱ���������Ҳ����Collider2D�������ͬ��������Ϊ��������
    // ��ô�뿪����Ϸ�����ϵĽű��е�OnTriggerExit2D������������
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

        // clone�忪ʼ�ͷŹ������ٴν���blackhole��enemy���ᵼ�´���hotkey���Ӷ���ɾ��hotkeyʱ����������
        if(!canCreateHotKey)
        {
            return;
        }

        // ������hotkeyʵ���ڵ�ǰλ�õ����Ϸ�+2��
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);
        // ��keyCodeList�����ѡȡһ��hotkey
        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        // ��keyCodeListɾ����key
        keyCodeList.Remove(choosenKey);
        // ��ȡhotkeyԤ�Ƽ��ϵ�controller
        BlackholeHotKeyController blackholeHotKeyController = newHotKey.GetComponent<BlackholeHotKeyController>();
        // ���ø�hotkey
        blackholeHotKeyController.SetupHotKey(choosenKey, collision.transform, this);
    }

    // �������transform���뵽Ŀ���б���
    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
