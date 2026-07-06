using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder_Attack_Trigger : MonoBehaviour
{
    [SerializeField] private Enemy_Behaviour_Exploder exploderBehaviour;

    public void AttackTrigger()
    {
        exploderBehaviour.DealDamage();
    }
}
