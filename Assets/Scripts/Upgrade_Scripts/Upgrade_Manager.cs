using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade_Manager : MonoBehaviour
{
    public Upgrade_Slot[] upgradeSlots;
    public Upgrade_SO[] upgradeSOs;

    private void OnEnable()
    {
        //subscribe to end battle upgrade sequence
    }

    private void OnDisable()
    {
        
    }

    private void ShowUpgrade()
    {
        foreach (var upgradeSlot in upgradeSlots)
        {
            upgradeSlot.PopulateSlot();
        }
    }

    private void UpgradeRandomize()
    {

    }
}
