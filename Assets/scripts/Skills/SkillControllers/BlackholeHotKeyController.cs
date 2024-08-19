using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackholeHotKeyController : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private BlackholeSkillController blackhole;

    public void SetupHotKey(KeyCode _myHotKey, Transform _myEnemy, BlackholeSkillController _myBlackhole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = _myEnemy;
        blackhole = _myBlackhole;

        myText.text = _myHotKey.ToString();
        myHotKey = _myHotKey;
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            blackhole.AddEnemyToList(myEnemy);

            // Color.clear完全透明的颜色
            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
