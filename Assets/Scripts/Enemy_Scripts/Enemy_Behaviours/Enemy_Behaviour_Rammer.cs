using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Behaviour_Rammer : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public float attackRange;
    [SerializeField] public float attackCooldown;

    private float attackCooldownTimer;
    private bool isAttacking;

    [SerializeField] private Enemy_SO enemySO;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform player;
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;

    private Vector2 attackDirection;
    private bool hasDealtDamage;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        attackCooldownTimer -= Time.deltaTime;
        SetState();
    }

    private void SetState()
    {
        if (isAttacking) return;

        if (Vector2.Distance(transform.position, player.position) < attackRange && attackCooldownTimer <= 0)
        {
            Charging();
        }
        else if (Vector2.Distance(transform.position, player.position) < attackRange)
        {
            rb.velocity = Vector2.zero;
            anim.Play("Idle");
        }
        else
        {
            Chase();
        }
    }

    private void Charging()
    {
        if (isAttacking) return;
        rb.velocity = Vector2.zero;
        anim.Play("Charging");
        isAttacking = true;
    }

    public void Attack()
    {
        hasDealtDamage = false;
        StartCoroutine(Dash());
    }
    private IEnumerator Dash()
    {
        attackDirection = (player.position - transform.position).normalized; //Lock direction
        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

        float timer = 0f;
        while (timer < dashDuration)
        {
            rb.velocity = attackDirection * dashSpeed; // reassert every physics step, overrides any push
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.velocity = Vector2.zero;
        transform.rotation = Quaternion.identity; // reset rotation back to normal
        attackCooldownTimer = attackCooldown;
        isAttacking = false;
        anim.Play("Idle");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAttacking || hasDealtDamage) return;

        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            collision.gameObject.GetComponent<Player_HP>().ChangeHP(enemySO.enemyDamage);
            Debug.Log($"Dealt {enemySO.enemyDamage} damage to {collision}");
            hasDealtDamage = true;
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
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}