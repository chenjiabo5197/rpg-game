using UnityEngine;

public class ShockStrike_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;
    private int damage;

    private Animator anim;
    private bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Setup(int _damage, CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetStats)
        {
            return;
        }

        if (triggered)
        {
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;

        if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            anim.transform.localPosition = new Vector3(0, .5f);
            // 将动画的旋转四元数变为无旋转的四元数，则动画为竖直状态 
            anim.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;

            // 将当前对象的缩放比例设置为X轴和Y轴都为3，而Z轴保持默认缩放比例（通常默认为1)
            transform.localScale = new Vector3(3, 3);

            Invoke("DamageAndSelfDestroy", .2f);
            triggered = true;
            anim.SetTrigger("Hit");
        }
    }

    private void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(1);
        // 延迟0.4s后删除该对象
        Destroy(gameObject, .4f);
    }
}
