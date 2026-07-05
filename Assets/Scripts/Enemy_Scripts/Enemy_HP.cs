using UnityEngine;

public class Enemy_HP : MonoBehaviour
{
    [SerializeField] private Enemy_SO enemySO;
    public float currentHP;

    public delegate void EnemyDefeated(float exp);
    public static event EnemyDefeated OnEnemyDefeated;

    public void Start()
    {
        currentHP = enemySO.enemyHP;
    }

    public void ChangeHP(float amount)
    {
        currentHP -= amount;

        if (currentHP > enemySO.enemyHP)
        {
            currentHP = enemySO.enemyHP;
        }
        else if (currentHP <= 0)
        {
            OnEnemyDefeated?.Invoke(enemySO.expReward);
            Destroy(gameObject);
        }
    }
}
