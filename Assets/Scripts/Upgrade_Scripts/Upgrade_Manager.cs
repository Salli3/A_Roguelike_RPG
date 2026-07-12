using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Upgrade_Manager : MonoBehaviour
{
    [SerializeField] private Upgrade_Slot[] upgradeSlots;
    [SerializeField] private Upgrade_UI upgradeUI;

    [SerializeField] private List<Owned_Slot> ownedSlots;
    [SerializeField] private GameObject ownedSlotPrefab;
    [SerializeField] private Transform ownedContainer;
    [SerializeField] private Stats_UI statsUI;

    private void OnEnable()
    {
        Upgrade_Slot.OnUpgradeChosen += ChooseUpgrade;
    }

    private void OnDisable()
    {
        Upgrade_Slot.OnUpgradeChosen -= ChooseUpgrade;
    }

    private void Start()
    {
        foreach (var slot in ownedSlots)
        {
            Destroy(slot.gameObject);
        }
        ownedSlots.Clear();
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
        List<Upgrade_SO> pool = new List<Upgrade_SO>(Stats_Manager.instance.upgradeSOs);

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
        //find what upgrade is being chosen
        string upgradeName = upgradeSO.upgradeName;

        //call to the stats manager to change stats
        switch (upgradeName)
        {
            case "HP Up":
                Stats_Manager.instance.UpdateMaxHP();
                break;
            case "Damage Up":
                Stats_Manager.instance.UpdateDamage();
                break;
            case "Attack Up":
                Stats_Manager.instance.UpdateAttackSpeed();
                break;
            case "Speed Up":
                Stats_Manager.instance.UpdateMovementSpeed();
                break;
            case "Exp Up":
                Stats_Manager.instance.UpdateExpGain();
                break;
            case "Money Up":
                Stats_Manager.instance.UpdateMoneyGain();
                break;
            default:
                Debug.LogWarning("Unknown skill: " + upgradeName);
                break;
        }
        AddUpgrade(upgradeSO);
        upgradeUI.SubtractPoint();
        statsUI.UpdateAllStats();
    }

    public void AddUpgrade(Upgrade_SO upgradeSO)
    {
        foreach (var slot in ownedSlots)
        {
            //Check if there is another same upgrade in owned
            if (slot.upgradeSO == upgradeSO)
            {
                slot.level++;
                slot.UpdateUI();
                return;
            }
        }
        AddNewSlot(upgradeSO);
    }

    private void AddNewSlot(Upgrade_SO upgradeSO)
    {
        Owned_Slot newSlot = Instantiate(ownedSlotPrefab, ownedContainer).GetComponent<Owned_Slot>();
        newSlot.Init(upgradeSO);
        ownedSlots.Add(newSlot);
    }
}
