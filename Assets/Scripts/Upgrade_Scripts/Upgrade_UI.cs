using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade_UI : MonoBehaviour
{
    [SerializeField] private List<GameObject> levelUpIcons = new List<GameObject>();
    [SerializeField] private GameObject levelUpIconPrefab;
    [SerializeField] private Transform levelUpPointContainer;
    [SerializeField] private CanvasGroup upgradeCanvasGroup;
    [SerializeField] private CanvasGroup parentCanvasGroup;
    [SerializeField] private Upgrade_Manager upgradeManager;
    [SerializeField] private Animator anim;


    private void OnEnable()
    {
        Exp_Manager.OnLevelUp += AddNewPoint;
        Enemy_HP.OnBattleEnd += ShowUpgrade;
    }

    private void OnDisable()
    {
        Exp_Manager.OnLevelUp -= AddNewPoint;
        Enemy_HP.OnBattleEnd -= ShowUpgrade;
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
        parentCanvasGroup.alpha = 0;
        parentCanvasGroup.interactable = false;
        parentCanvasGroup.blocksRaycasts = false;
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
        parentCanvasGroup.alpha = 1;
        parentCanvasGroup.interactable = true;
        parentCanvasGroup.blocksRaycasts = true;

        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            anim.Play("Upgrade_Panel_Fade_In");
            StartCoroutine(ShowUpgradePanel());
        }
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
            anim.Play("Idle");
            parentCanvasGroup.alpha = 0;
            parentCanvasGroup.interactable = false;
            parentCanvasGroup.blocksRaycasts = false;
        }
    }

    private IEnumerator ShowUpgradePanel()
    {
        yield return new WaitForSecondsRealtime(1);

        upgradeCanvasGroup.alpha = 1;
        upgradeCanvasGroup.interactable = true;
        upgradeCanvasGroup.blocksRaycasts = true;
    }
}