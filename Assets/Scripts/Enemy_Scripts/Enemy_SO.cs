using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO")]
public class Enemy_SO : ScriptableObject
{
    public string enemyName;
    public float enemyHP;
    public float enemyDamage;
    public int dangerLevel;
}