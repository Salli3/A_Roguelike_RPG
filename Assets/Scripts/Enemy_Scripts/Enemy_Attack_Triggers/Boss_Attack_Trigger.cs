using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Attack_Trigger : MonoBehaviour
{
    [SerializeField] private Boss_Behaviour bossBehaviour;

    public void AttackTrigger()
    {
        switch (bossBehaviour.currentState)
        {
            case Boss_Behaviour.EnemyState.MeleeAttack:
                bossBehaviour.FinishMeleeAttack();
                break;

            case Boss_Behaviour.EnemyState.RangedAttack:
                bossBehaviour.FireProjectile();
                break;

            case Boss_Behaviour.EnemyState.SpecialAttack:
                bossBehaviour.LaunchDash();
                break;
        }
    }
}
