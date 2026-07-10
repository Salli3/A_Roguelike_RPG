using System;
using System.Collections;
using UnityEngine;

public class Enemy_HP : MonoBehaviour
{
    public Enemy_SO enemySO;
    public float currentHP;

    [SerializeField] private SpriteRenderer currentSprite;

    public delegate void EnemyDefeated(float exp);
    public static event EnemyDefeated OnEnemyDefeated;
    public static event Action OnBattleEnd;


    public void Start()
    {
        currentHP = enemySO.enemyHP;
        currentSprite.sprite = enemySO.enemySprite;
        if (enemySO.isBoss)
        {
            Game_Manager.instance.bossUI.ShowUI(this, enemySO);
            Game_Manager.instance.bossOnScreen++;
        }
        Game_Manager.instance.enemyOnScreen++;
    }

    public void ChangeHP(float amount)
    {
        currentHP -= amount;
        StartCoroutine(enemyHitSpriteChange());

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

            Game_Manager.instance.enemyOnScreen--;
            OnEnemyDefeated?.Invoke(enemySO.expReward);
            Destroy(gameObject);

            if (Game_Manager.instance.enemyOnScreen == 0)
            {
                Debug.Log("Battle ended");
                OnBattleEnd?.Invoke();
            }
            return;
        }

        if (enemySO.isBoss)
        {
            Game_Manager.instance.bossUI.UpdateUI(this, enemySO);
        }    
    }
    private IEnumerator enemyHitSpriteChange()
    {
        GetComponentInChildren<Animator>().enabled = false;
        Debug.Log("Change Enemy Sprite");
        currentSprite.sprite = enemySO.enemyHitSprite;
        yield return new WaitForSeconds(0.1f);        
        currentSprite.sprite = enemySO.enemySprite;
        GetComponentInChildren<Animator>().enabled = true;
    }
}
