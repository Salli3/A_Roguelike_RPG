using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade_Slot : MonoBehaviour
{
    [SerializeField] private Upgrade_SO upgradeSO;
    [SerializeField] private Image upgradeImage;
    [SerializeField] private TMP_Text upgradeNameText;
    [SerializeField] private TMP_Text upgradeDescriptionText;

    public void PopulateSlot()
    {
        if (upgradeSO != null)
        {
            upgradeImage.sprite = upgradeSO.icon;
            upgradeNameText.text = upgradeSO.upgradeName;
            upgradeDescriptionText.text = upgradeSO.description;
        }
    }
}
