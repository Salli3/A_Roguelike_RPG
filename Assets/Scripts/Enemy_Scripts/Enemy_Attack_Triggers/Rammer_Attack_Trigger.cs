using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rammer_Attack_Trigger : MonoBehaviour
{
    [SerializeField] private Enemy_Behaviour_Rammer rammerBehaviour;

    public void AttackTrigger()
    {
        rammerBehaviour.Attack();
    }
}
