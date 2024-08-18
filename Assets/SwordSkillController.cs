using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    // ������ʱ���ٶ�
    [SerializeField] private float returnSpeed = 12;
    private Animator anim;
    private Rigidbody2D rb;
    // �ɳ�ȥ�Ľ�����ײ��
    private CircleCollider2D cc;
    private Player player;
    // ���Ƿ���Ҫ��ת
    private bool canRotate = true;
    // ���Ƿ������
    private bool isReturning;

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
        if(canRotate)
        {
            transform.right = rb.velocity;
        }

        if(isReturning)
        {
            // �ڶ�ά�ռ���ƽ���ش�һ�����ƶ�����һ����, ������ ��ǰλ�ã�Ŀ��λ�ã��ڵ�ǰ֡�У���������ƶ���������
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1 )
            {
                player.ClearTheSword();
            }
        }
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player)
    {
        player = _player;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        anim.SetBool("Rotation", true);
    }

    // ���ս�
    public void ReturnSword()
    {
        rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    // ��һ����Ϸ����� Collider2D����ײ��������һ����Ϸ����� Trigger2D�����������Ӵ�ʱ���ᴥ���������
    // ע��Ҫ�޸�sword��CircleCollider2D��istriggerֵΪtrue����sword����ײ�����Դ����ú���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetBool("Rotation", false);
        // �رս�����ת
        canRotate = false;
        // ���ڿ�����ײ���Ƿ����������ײ�ʹ������¼�, Ϊ false ʱ����ײ�����������ײ���򴥷����¼�
        cc.enabled = false;
        // ָ�� Rigidbody2D �Ƿ��������������, ����Ϊ true ʱ���� Rigidbody2D �������ܵ������������������������ײ���ȣ���Ӱ�죬Ҳ�������������ײ���Զ����
        rb.isKinematic = true;
        // constraints ��������������Ϸ�����ڶ�ά���������е��ƶ�����ת,
        // RigidbodyConstraints2D.FreezeAll��ֵ�ᶳ�ᣨ��Լ������Ϸ�����ڶ�ά���������е������ƶ�����ת���ɶ�
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;
    }
}
