using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Exp_Manager : MonoBehaviour
{
    [SerializeField] private Slider expSlider;
    [SerializeField] private TMP_Text levelText;

    public static event Action<int> OnLevelUp;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GainExp(10);
        }
    }

    private void GainExp(float amount)
    {
        Stats_Manager.instance.currentExp += amount * Stats_Manager.instance.expGain;
        while (Stats_Manager.instance.currentExp >= Stats_Manager.instance.expToLevel)
        {
            LevelUp();
        }
        UpdateUI();
    }

    private void LevelUp()
    {
        Stats_Manager.instance.level++;
        Stats_Manager.instance.currentExp -= Stats_Manager.instance.expToLevel;
        Stats_Manager.instance.expToLevel = Stats_Manager.instance.expToLevel * Stats_Manager.instance.expGrowthMultiplier;
        OnLevelUp?.Invoke(1);
    }

    private void UpdateUI()
    {
        expSlider.maxValue = Stats_Manager.instance.expToLevel;
        expSlider.value = Stats_Manager.instance.currentExp;
        levelText.text = "LV: " + Stats_Manager.instance.level;
    }
}
