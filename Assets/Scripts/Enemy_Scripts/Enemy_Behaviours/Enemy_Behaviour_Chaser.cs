using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public float attackRange;
    [SerializeField] public float attackCooldown;

    private float attackCooldownTimer;
    private bool isAttacking;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform player;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        attackCooldownTimer -= Time.deltaTime;

        Attack();
    }

    private void Chase()
    {
        if (player.position.x > transform.position.x && transform.localScale.x < 0 ||
            player.position.x < transform.position.x && transform.localScale.x > 0)
        {
            Flip();
        }
        anim.Play("Idle");
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    private void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void Attack()
    {
        if (Vector2.Distance(attackPoint.position, player.position) < attackRange)
        {
            rb.velocity = Vector2.zero;
            anim.Play("Attack");
        }
        else
        {
            Chase();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}


