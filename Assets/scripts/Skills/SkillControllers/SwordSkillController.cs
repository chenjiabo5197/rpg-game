using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* ����Ԥ�Ƽ���������Ϊenable״̬��������ʵ�����󲻻�ִ�к�awake��start�ȷ������˴����ûᱨ��ָ���쳣�ȴ���
* Ԥ�Ƽ�������samplescene��ɾ����������prefabs�ļ����е�Ԥ�Ƽ�������enable״̬
*/

public class SwordSkillController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    // �ɳ�ȥ�Ľ�����ײ��
    private CircleCollider2D cc;
    private Player player;
    // ���Ƿ���Ҫ��ת
    private bool canRotate = true;
    // ���Ƿ������
    private bool isReturning;

    // ���ӳ�ȥ�Ľ����к󶳽��ʱ��
    private float freezeTimeDuration;
    // ������ʱ���ٶ�
    private float returnSpeed = 12;

    [Header("Pierce info")]
    [SerializeField] private float pierceAmount;

    [Header("Bounce info")]
    // ���ķ����ٶ�
    private float bounceSpeed;
    // �ӳ�ȥ��sword�Ƿ��ڷ�����
    private bool isBouncing;
    // ������������
    private int bounceAmount;
    // �ڽ�������Χ�ڵ�enemy
    private List<Transform> enemyTarget;
    // ����enemy������
    private int targetIndex;

    [Header("Spin info")]
    // player����ת�Ľ�֮��������
    private float maxTravelDistance;
    // ��ת��ʱ��
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpining;
    // ��ת���Ĺ�����ʱ��
    private float hitTimer;
    // ��ת���Ĺ���Ƶ�ʣ�ÿ�ι������ΪhitCooldown
    private float hitCooldown;

    private float spinDirection;

    private void Awake()
    {
        // ע�⣬�˴��ŵ�start�����ֿ�ָ���쳣
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {

    }

    public void Update()
    {
        if (canRotate)
        {
            // ��ʱ�����ҷ�����ٶ�Ϊ�����ƶ��ٶȣ��������ÿ����ý��ڻ�������ʱ����б���²���״̬������ƽ����ˮƽ������״̬
            transform.right = rb.velocity;
        }

        if (isReturning)
        {
            // �ڶ�ά�ռ���ƽ���ش�һ�����ƶ�����һ����, ������ ��ǰλ�ã�Ŀ��λ�ã��ڵ�ǰ֡�У���������ƶ���������
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchTheSword();
            }
        }

        BounceLogic();

        SpinLogic();
    }
    
    // �����ӳ�ȥ�Ľ�����
    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed)
    {
        player = _player;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
        if(pierceAmount <=0 )
        {
            anim.SetBool("Rotation", true);
        }

        // ��һ��ֵ(rb.velocity.x)����������ָ������Сֵ��-1�������ֵ��1��֮��
        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        // ��ָ�����ӳ�ʱ����Զ�����һ������
        Invoke("DestroyMe", 7);
    }

    private void SpinLogic()
    {
        if (isSpining)
        {
            // ��sword��player�ľ������ʱ
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpining();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                // ����ת��swordһֱ����������ƶ�
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f *  Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpining = false;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (Collider2D collider in colliders)
                    {
                        if (collider.GetComponent<Enemy>() != null)
                        {
                            SwordSkillDamage(collider.GetComponent<Enemy>());
                        }
                    }
                }
            }
        }
    }

    // ֹͣsword������Զ���ߣ��ڴ˾��봦��ʼ��ת
    private void StopWhenSpining()
    {
        wasStopped = true;
        // ֹͣsword��λ�ƣ���sword�ڴ˴���ת
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        // �жϷ������б����Ƿ�Ϊ�գ���Ϊ���Ҵ��ڷ����У���ʼ����
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }


    public void SetupBounce(bool _isBouncing, int _amountBounces, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountBounces;
        bounceSpeed = _bounceSpeed;

        // enemyTarget��private��������Ҫ�ֶ������������public������Ҫ��һ�䣬unity���Զ�����
        enemyTarget = new List<Transform>();
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupSpin(bool _isSpining, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpining = _isSpining;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    // ���ս�
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;   // �޸��ӳ�ȥ�Ľ�������ȴ���Ȼ����ܷ��ص�bug
        transform.parent = null;
        isReturning = true;
    }

    // ��һ����Ϸ����� Collider2D����ײ��������һ����Ϸ����� Trigger2D�����������Ӵ�ʱ���ᴥ���������
    // ע��Ҫ�޸�sword��CircleCollider2D��istriggerֵΪtrue����sword����ײ�����Դ����ú���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
        {
            return;
        }

        if(collision.GetComponent<Enemy>()!=null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        /*// ?.��������������Ϊnull���������������Ա���������Ϊnull���򲻻᳢�Է������Ա�������������ʽ�Ľ��Ϊnull
        collision.GetComponent<Enemy>()?.Damage();*/
        SetupTargetForBounce(collision);

        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        enemy.Damage();
        enemy.StartCoroutine("FreezeTimerFor", freezeTimeDuration);
    }

    private void SetupTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)    // ����enemy��Żᷴ��
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (Collider2D collider in colliders)
                {
                    // �ڷ����İ뾶��Χ�ڶ�����enemy�ļ��뵽�б���
                    if (collider.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(collider.transform);
                    }
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        // ��תsword���Ῠ���κζ�����
        if(isSpining)
        { 
            StopWhenSpining();  // �ӳ�ȥ��ת�Ľ�������һ��enmeyʱֹͣ���ڴ˴���ת
            return; 
        }

        // �رս�����ת
        canRotate = false;
        // ���ڿ�����ײ���Ƿ����������ײ�ʹ������¼�, Ϊ false ʱ����ײ�����������ײ���򴥷����¼�
        cc.enabled = false;
        // ָ�� Rigidbody2D �Ƿ��������������, ����Ϊ true ʱ���� Rigidbody2D �������ܵ������������������������ײ���ȣ���Ӱ�죬Ҳ�������������ײ���Զ����
        rb.isKinematic = true;
        // constraints ��������������Ϸ�����ڶ�ά���������е��ƶ�����ת,
        // RigidbodyConstraints2D.FreezeAll��ֵ�ᶳ�ᣨ��Լ������Ϸ�����ڶ�ά���������е������ƶ�����ת���ɶ�
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
        {
            // ���ڷ����в��ر���ת�������ø�����
            return;
        }

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
