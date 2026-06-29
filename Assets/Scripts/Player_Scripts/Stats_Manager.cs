using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stats_Manager : MonoBehaviour
{
    public static Stats_Manager instance;

    [Header("Player Combat Stats")]

    public float attackRange;
    public float damage;
    public float knockbackForce;
    public float knockbackTime;
    public float stunTime;
    public float cooldown;
    //TODO crit

    [Header("Player Movement Stats")]

    public float speed;

    [Header("Player Hp Stats")]

    public float currentHP;
    public float maxHP;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
