using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rammer_Attack_Trigger : MonoBehaviour
{
    [SerializeField] private Enemy_Behaviour_Rammer chaserBehaviour;

    public void AttackTrigger()
    {
        chaserBehaviour.Attack();
    }
}
