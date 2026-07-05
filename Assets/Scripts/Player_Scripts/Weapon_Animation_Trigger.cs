using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Animation_Trigger : MonoBehaviour
{
    [SerializeField] private Player_Combat playerCombat;

    public void AttackTrigger()
    {
        playerCombat.TriggerAttack();
    }
}
