using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Owned_Slot : MonoBehaviour
{
    public Upgrade_SO upgradeSO;
    public int level;

    [SerializeField] private Image ownedImage;
    [SerializeField] private TMP_Text levelText;

    public void Init(Upgrade_SO newUpgradeSO)
    {
        upgradeSO = newUpgradeSO;
        level = 1;
        UpdateUI();
    }

    public void UpdateUI()
    {
        ownedImage.sprite = upgradeSO.icon;
        levelText.text = "LV: " + level;
    }
}
