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
        if(enemySO.isBoss)
        {
            Game_Manager.instance.bossUI.ShowUI(this, enemySO);
            Game_Manager.instance.bossOnScreen++;
        }
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
            if (enemySO.isBoss)
            {
                Game_Manager.instance.bossUI.UpdateUI(this, enemySO);
                Game_Manager.instance.bossOnScreen--;
                if (Game_Manager.instance.bossOnScreen == 0)
                {
                    Game_Manager.instance.bossUI.HideUI();
                }
            }
            OnEnemyDefeated?.Invoke(enemySO.expReward);
            Destroy(gameObject);
            return;
        }

        if (enemySO.isBoss)
        {
            Game_Manager.instance.bossUI.UpdateUI(this, enemySO);
        }
    }
}
