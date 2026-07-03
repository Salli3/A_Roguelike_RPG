using System.Collections;
using UnityEngine;

public class Enemy_Behaviour_Chaser : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public float attackRange;
    [SerializeField] public float attackCooldown;
    [SerializeField] private float attackPointDistance = 1;

    private float attackCooldownTimer;
    private bool isAttacking;

    [SerializeField] private Enemy_SO enemySO;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform player;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask playerLayer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        attackCooldownTimer -= Time.deltaTime;

        if (isAttacking == false)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            attackPoint.position = (Vector2)transform.position + direction * 1;
        }

        SetState();
    }

    private void SetState()
    {
        if (Vector2.Distance(attackPoint.position, player.position) < attackRange / 2 && attackCooldownTimer <= 0)
        {
            Debug.Log("Attacking");
            Attack();
        }
        else if (Vector2.Distance(attackPoint.position, player.position) < attackRange / 2 && isAttacking == false)
        {
            rb.velocity = Vector2.zero;
            anim.Play("Idle");
        }
        else if (isAttacking == false)
        {
            Chase();
        }
    }

    private void Attack()
    {
        rb.velocity = Vector2.zero;
        attackCooldownTimer = attackCooldown;
        anim.Play("Attack");
        isAttacking = true;
        StartCoroutine(FinishAttacking());
    }

    IEnumerator FinishAttacking()
    {
        yield return new WaitForSeconds(0.6f);
        isAttacking = false;
        DealDamage();
    }

    private void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        if (hits.Length > 0)
        {
            hits[0].GetComponent<Player_HP>().ChangeHP(enemySO.enemyDamage);
            Debug.Log($"Dealt {enemySO.enemyDamage} damage to {hits[0]}");
        }
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
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, attackPointDistance);
    }
}


