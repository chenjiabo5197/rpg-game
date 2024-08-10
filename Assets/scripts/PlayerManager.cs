using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;

    private void Awake()
    {
        if (instance != null)
        {
            // 需要实例化多个，则删除后面的实例对象，确保只会实例化一个对象
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
