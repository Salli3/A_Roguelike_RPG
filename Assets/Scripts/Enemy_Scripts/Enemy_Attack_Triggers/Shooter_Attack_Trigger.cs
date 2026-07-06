using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_Attack_Trigger : MonoBehaviour
{
    [SerializeField] private Enemy_Behaviour_Shooter shooterBehaviour;

    public void AttackTrigger()
    {
        shooterBehaviour.FinishShooting();
    }
}
