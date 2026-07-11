using UnityEngine;

[CreateAssetMenu(fileName = "ClassSO")]
public class Class_SO : ScriptableObject
{
    [Header("General Info")]
    public string className;
    public float damage;
    public float maxHP;
    public float speed;
    public float attackSpeed;
    public float attackRange;
    public Sprite classSprite;
    public Sprite classHitSprite;

    [Header("Combat Settings")]
    public bool isRanged;                
    public Sprite projectileSprite;
    public Sprite weaponSprite;

    [Header("Class Upgrades")]
    public Upgrade_SO[] upgradeSOs;
}
