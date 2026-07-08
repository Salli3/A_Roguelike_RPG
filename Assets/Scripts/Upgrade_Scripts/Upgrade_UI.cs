using System;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade_UI : MonoBehaviour
{
    [SerializeField] private List<GameObject> levelUpIcons = new List<GameObject>();
    [SerializeField] private GameObject levelUpIconPrefab;
    [SerializeField] private Transform levelUpPointContainer;
    [SerializeField] private CanvasGroup upgradeCanvasGroup;
    [SerializeField] private Upgrade_Manager upgradeManager;


    private void OnEnable()
    {
        Exp_Manager.OnLevelUp += AddNewPoint;
        Upgrade_Manager.OnBattleEnd += ShowUpgrade;
    }

    private void OnDisable()
    {
        Exp_Manager.OnLevelUp -= AddNewPoint;
        Upgrade_Manager.OnBattleEnd -= ShowUpgrade;
    }

    private void Start()
    {
        foreach (var icon in levelUpIcons)
        {
            Destroy(icon);
        }
        levelUpIcons.Clear();

        upgradeCanvasGroup.alpha = 0;
        upgradeCanvasGroup.interactable = false;
        upgradeCanvasGroup.blocksRaycasts = false;
    }

    //Add new point indicator when level up
    private void AddNewPoint()
    {
        GameObject newIcon = Instantiate(levelUpIconPrefab, levelUpPointContainer);
        levelUpIcons.Add(newIcon);
    }

    //Show upgrade panel when battle end && have point
    private void ShowUpgrade()
    {
        if (levelUpIcons.Count == 0) return;
        upgradeManager.PopulateSlots();
        upgradeCanvasGroup.alpha = 1;
        upgradeCanvasGroup.interactable = true;
        upgradeCanvasGroup.blocksRaycasts = true;
        Time.timeScale = 0;
    }

    //Continue upgrade sequence or stop if out of point
    public void SubtractPoint()
    {
        int lastIndex = levelUpIcons.Count - 1;
        GameObject iconToRemove = levelUpIcons[lastIndex];

        levelUpIcons.RemoveAt(lastIndex);
        Destroy(iconToRemove);

        //Check if another round of upgrade sequence is needed or not
        if (levelUpIcons.Count > 0)
        {
            ShowUpgrade();
        }
        else
        {
            upgradeCanvasGroup.alpha = 0;
            upgradeCanvasGroup.interactable = false;
            upgradeCanvasGroup.blocksRaycasts = false;
            Time.timeScale = 1;
        }
    }
}