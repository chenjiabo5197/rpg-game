using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{

    // 获取摄像机，背景图可以跟随这个摄像机运动
    private GameObject cam;

    // 视差影响，即相对于摄像机的移动速度的倍数
    [SerializeField] private float parallaxEffect;

    // 背景的x坐标
    private float xPosition;
    // 当前背景图的长度
    private float length;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);
        // 相机的x坐标*视差影响为背景x坐标需要移动的距离
        float distanceToMove = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if(distanceMoved > xPosition + length)
        {
            xPosition = xPosition + length;
        }
        else if(distanceMoved < xPosition - length)
        {
            xPosition = xPosition - length;
        }
    }
}
