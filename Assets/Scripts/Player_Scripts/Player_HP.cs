using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_HP : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private Animator hpBarAnim;
    [SerializeField] private Slider hpBar;

    private void Start()
    {
        hpText.text = Stats_Manager.instance.currentHP + "/" + Stats_Manager.instance.maxHP;
        UpdateUI();
    }

    public void ChangeHP(float amount)
    {
        Stats_Manager.instance.currentHP -= amount;
        hpBarAnim.Play("HP_Change");
        hpText.text = Stats_Manager.instance.currentHP + "/" + Stats_Manager.instance.maxHP;

        if (Stats_Manager.instance.currentHP <= 0)
        {
            gameObject.SetActive(false);
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        hpBar.maxValue = Stats_Manager.instance.maxHP;
        hpBar.value = Stats_Manager.instance.currentHP;
    }
}
