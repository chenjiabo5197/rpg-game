using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��unity�ڲ�����һ��create������Ŀ��·����Data/Item effect/Ice and Fire��������Ĭ����ΪIce and Fire effect
[CreateAssetMenu(fileName = "Ice and Fire effect", menuName = "Data/Item effect/Ice and Fire")]
public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject IceAndFirePrefab;
    [SerializeField] private float xVelocity;

    public override void ExecuteEffect(Transform _responsePosition)
    {
        Player player = PlayerManager.instance.player;

        bool thirdAttack = player.primaryAttackState.comboCounter == 2;

        // �������չ����б���Ч��
        if (thirdAttack) 
        { 
            GameObject newIceAndFire = Instantiate(IceAndFirePrefab, _responsePosition.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir, 0);

            Destroy(newIceAndFire, 10);
        }
    }  
}
