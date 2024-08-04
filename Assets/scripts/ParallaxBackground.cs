using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{

    // ��ȡ�����������ͼ���Ը������������˶�
    private GameObject cam;

    // �Ӳ�Ӱ�죬���������������ƶ��ٶȵı���
    [SerializeField] private float parallaxEffect;

    // ������x����
    private float xPosition;
    // ��ǰ����ͼ�ĳ���
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
        // �����x����*�Ӳ�Ӱ��Ϊ����x������Ҫ�ƶ��ľ���
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
