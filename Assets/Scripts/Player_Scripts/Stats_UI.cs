using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stats_UI : MonoBehaviour
{
    public GameObject[] statsSlots;

    private void Start()
    {
        UpdateAllStats();
    }

    public void UpdateAllStats()
    {
        UpdateMaxHP();
        UpdateDamage();
        UpdateAttackSpeed();
        UpdateSpeed();
        UpdateExpGain();
        UpdateMoneyGain();

    }

    public void UpdateMaxHP()
    {
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = ((float)Math.Round(Stats_Manager.instance.maxHP, 2)).ToString();
    }
    public void UpdateDamage()
    {
        statsSlots[1].GetComponentInChildren<TMP_Text>().text = ((float)Math.Round(Stats_Manager.instance.damage, 2)).ToString();
    }
    public void UpdateAttackSpeed()
    {
        statsSlots[2].GetComponentInChildren<TMP_Text>().text = ((float)Math.Round(Stats_Manager.instance.attackSpeed, 2)).ToString();
    }
    public void UpdateSpeed()
    {
        statsSlots[3].GetComponentInChildren<TMP_Text>().text = ((float)Math.Round(Stats_Manager.instance.speed, 2)).ToString();
    }
    public void UpdateExpGain()
    {
        statsSlots[4].GetComponentInChildren<TMP_Text>().text = ((float)Math.Round(Stats_Manager.instance.expGain, 2)).ToString();
    }
    public void UpdateMoneyGain()
    {
        statsSlots[5].GetComponentInChildren<TMP_Text>().text = ((float)Math.Round(Stats_Manager.instance.moneyGain, 2)).ToString();
    }
}
