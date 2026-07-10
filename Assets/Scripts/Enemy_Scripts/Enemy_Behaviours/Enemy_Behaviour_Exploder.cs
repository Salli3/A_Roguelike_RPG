using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Behaviour_Exploder : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public float attackRange;
    private bool isAttacking = false;

    [SerializeField] private Enemy_SO enemySO;
    [SerializeField] private Enemy_Knockback enemyKnockback;
    [SerializeField] private Enemy_HP enemyHP;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform player;
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask playerLayer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        SetState();
    }

    private void SetState()
    {
        if (isAttacking) return;

        if (enemyKnockback.isKnockedback)
        {
            anim.Play("Idle");
            return;
        }

        if (Vector2.Distance(transform.position, player.position) < attackRange / 2)
        {
            Attack();
        }
        else
        {
            Chase();
        }
    }

    private void Attack()
    {
        rb.velocity = Vector2.zero;
        anim.Play("Exploding");
        isAttacking = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }
    
    public void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);

        if (hits.Length > 0)
        {
            hits[0].GetComponent<Player_HP>().ChangeHP(enemySO.enemyDamage);
            Debug.Log($"Dealt {enemySO.enemyDamage} damage to {hits[0]}");
        }

        enemyHP.ChangeHP(enemySO.enemyHP);
    }

    private void Chase()
    {
        if (player.position.x > transform.position.x && transform.localScale.x < 0 ||
            player.position.x < transform.position.x && transform.localScale.x > 0)
        {
            Flip();
        }
        anim.Play("Chase");
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    private void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}