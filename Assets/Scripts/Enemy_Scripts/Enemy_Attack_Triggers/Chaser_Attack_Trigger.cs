using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser_Attack_Trigger : MonoBehaviour
{
    [SerializeField] private Enemy_Behaviour_Chaser chaserBehaviour;

    public void AttackTrigger()
    {
        chaserBehaviour.DealDamage();
    }
}
