using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade_Slot : MonoBehaviour
{
    [SerializeField] private Upgrade_SO upgradeSO;
    [SerializeField] private Image upgradeImage;
    [SerializeField] private TMP_Text upgradeNameText;
    [SerializeField] private TMP_Text upgradeDescriptionText;
    [SerializeField] private Button upgradeButton;

    public static event Action<Upgrade_SO> OnUpgradeChosen;

    private void Awake()
    {
        upgradeButton = GetComponent<Button>();
        upgradeButton.onClick.AddListener(OnUpgradeClicked);
    }

    //Show an Upgrade_Slot with the passing in Upgrade_SO
    public void ShowSlot(Upgrade_SO upgradeSO)
    {
        if (upgradeSO != null)
        {
            gameObject.SetActive(true);
            this.upgradeSO = upgradeSO;
            upgradeImage.sprite = upgradeSO.icon;
            upgradeNameText.text = upgradeSO.upgradeName;
            upgradeDescriptionText.text = upgradeSO.description;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    //An onclick trigger method to fire up an event to all its listener to pass the Upgrade_SO for their logic
    public void OnUpgradeClicked()
    {
        OnUpgradeChosen?.Invoke(upgradeSO);
    }
}
