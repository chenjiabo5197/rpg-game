using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    // 设置clone体的位置
    public void SetupClone(Transform _newTransform)
    {
        transform.position = _newTransform.position;
    }
}
