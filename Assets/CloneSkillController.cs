using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    // ����clone���λ��
    public void SetupClone(Transform _newTransform)
    {
        transform.position = _newTransform.position;
    }
}
