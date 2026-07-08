using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss_UI : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Animator hpBarAnim;
    [SerializeField] private Slider hpBar;
    [SerializeField] private CanvasGroup bossCanvasGroup;

    private void Start()
    {
        bossCanvasGroup.alpha = 0;
    }

    public void ShowUI(Enemy_HP enemyHP, Enemy_SO enemySO)
    {
        StopAllCoroutines();
        bossCanvasGroup.alpha = 1;
        hpBarAnim.Play("Show");
        nameText.text = enemySO.enemyName;
        hpText.text = enemyHP.currentHP + "/" + enemySO.enemyHP;
        hpBar.maxValue = enemySO.enemyHP;
        hpBar.value = enemyHP.currentHP;
    }

    public void HideUI()
    {
        hpBarAnim.Play("Hide");
        StartCoroutine(HideCanvas());
    }

    public void UpdateUI(Enemy_HP enemyHP, Enemy_SO enemySO)
    {
        nameText.text = enemySO.enemyName;
        hpText.text = Mathf.Max(0, enemyHP.currentHP) + "/" + enemySO.enemyHP;
        hpBar.maxValue = enemySO.enemyHP;
        hpBar.value = enemyHP.currentHP;
        hpBarAnim.Play("Update");
    }

    IEnumerator HideCanvas()
    {
        yield return new WaitForSeconds(5);
        bossCanvasGroup.alpha = 0;
    }
}
