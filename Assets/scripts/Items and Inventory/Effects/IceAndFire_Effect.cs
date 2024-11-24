using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 在unity内部新增一个create的新项目，路径是Data/Item effect/Ice and Fire，创建后默认名为Ice and Fire effect
[CreateAssetMenu(fileName = "Ice and Fire effect", menuName = "Data/Item effect/Ice and Fire")]
public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject IceAndFirePrefab;
    [SerializeField] private float xVelocity;

    public override void ExecuteEffect(Transform _responsePosition)
    {
        Player player = PlayerManager.instance.player;

        bool thirdAttack = player.primaryAttackState.comboCounter == 2;

        // 第三次普攻再有冰火效果
        if (thirdAttack) 
        { 
            GameObject newIceAndFire = Instantiate(IceAndFirePrefab, _responsePosition.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir, 0);

            Destroy(newIceAndFire, 10);
        }
    }  
}
