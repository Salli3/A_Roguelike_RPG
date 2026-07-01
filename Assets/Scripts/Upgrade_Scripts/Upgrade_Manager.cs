using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Upgrade_Manager : MonoBehaviour
{
    //TODO a reference to player Class_SO to pull the class Upgrade_SO list
    [SerializeField] private Upgrade_Slot[] upgradeSlots;
    [SerializeField] private Upgrade_SO[] upgradeSOs;
    [SerializeField] private Upgrade_UI upgradeUI;

    public static event Action OnBattleEnd;

    private void OnEnable()
    {
        Upgrade_Slot.OnUpgradeChosen += ChooseUpgrade;
    }

    private void OnDisable()
    {
        Upgrade_Slot.OnUpgradeChosen -= ChooseUpgrade;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Test UI"))
        {
            OnBattleEnd?.Invoke();
        }
    }

    //Get a random list from the Upgrade pool through a helper method and pass it to an array of Upgrade_Slot
    public void PopulateSlots()
    {
        //TODO make upgrade slot number check for number of option
        List<Upgrade_SO> randomUpgrades = GetRandomUpgrades(upgradeSlots.Length);

        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            upgradeSlots[i].ShowSlot(randomUpgrades[i]);
        }
    }

    //Helper method to get a random upgrade from a list that has not been selected yet in this upgrade sequence
    private List<Upgrade_SO> GetRandomUpgrades(int count)
    {
        List<Upgrade_SO> pool = new List<Upgrade_SO>(upgradeSOs);

        // Fisher-Yates shuffle
        for (int i = pool.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (pool[i], pool[j]) = (pool[j], pool[i]);
        }

        return pool.GetRange(0, Mathf.Min(count, pool.Count));
    }

    //Trigger after an upgrade is chosen, executing upgrade logic and call to Upgrade_UI to continue upgrade sequence
    private void ChooseUpgrade(Upgrade_SO upgradeSO)
    {
        //TODO upgrade logic
        upgradeUI.SubtractPoint();
    }
}
