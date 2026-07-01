using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stats_Manager : MonoBehaviour
{
    public static Stats_Manager instance;

    [Header("Player Class")]
    [SerializeField] private Class_SO currentClass;
    public bool isRanged;
    public Sprite projectileSprite;
    public Sprite weaponSprite;
    public Upgrade_SO[] upgradeSOs;

    [Header("Player Flexible Stats")]
    public float damage;
    public float currentHP;
    public float maxHP;
    public float cooldown;
    public float speed;
    //TODO crit

    [Header("Player Fixed Stats")]
    public float attackRange;
    public float knockbackForce;
    public float knockbackTime;
    public float stunTime;
    

    [Header("Player Exp Stats")]
    public int level;
    public float expGain;
    public float currentExp;
    public float expToLevel;
    public float expGrowthMultiplier = 2;

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
        RegisterStats(); // initialize stats from currentClass
    }

    public void SetClass(Class_SO newClass)
    {
        currentClass = newClass;
        RegisterStats();
    }

    private void RegisterStats()
    {
        if (currentClass == null)
        {
            Debug.LogWarning("No class assigned to StatsManager!");
            return;
        }

        // Flexible stats
        damage = currentClass.damage;
        maxHP = currentClass.maxHP;
        currentHP = maxHP; // start full
        cooldown = currentClass.cooldown;
        speed = currentClass.speed;

        // Fixed stats
        attackRange = currentClass.attackRange;
        knockbackForce = currentClass.knockbackForce;
        knockbackTime = currentClass.knockbackTime;
        stunTime = currentClass.stunTime;

        // Combat settings
        isRanged = currentClass.isRanged;
        projectileSprite = currentClass.projectileSprite;

        // Upgrades
        upgradeSOs = currentClass.upgradeSOs;

        Debug.Log($"Stats registered for {currentClass.className}");
    }
}
