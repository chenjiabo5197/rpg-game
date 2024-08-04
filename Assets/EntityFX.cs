using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    // 被击中的效果展示时间
    [SerializeField] private float flashDuration;
    // 被击中时的展示效果
    [SerializeField] private Material hitMat;
    // 原始的展示效果
    private Material originalMat;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    // 起一个协程，先将material置为打击时的material，等待一定时间后，再将其置为原始的material
    private IEnumerator FlashFX()
    {
        sr.material = hitMat;

        yield return new WaitForSeconds(flashDuration);

        sr.material = originalMat;
    }
}
