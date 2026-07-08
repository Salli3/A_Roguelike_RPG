using System.Collections;
using UnityEngine;

public class Boss_Behaviour : MonoBehaviour
{
    [Header("Behaviour properties")]
    [SerializeField] private float speed;
    [SerializeField] private float attackCooldown;

    [Header("Components reference")]
    [SerializeField] private Enemy_SO enemySO;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform player;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject enemyProjectilePrefab;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Sprite projectileSprite;
    [SerializeField] private LayerMask playerLayer;

    [Header("Melee Attack")]
    [SerializeField] private float meleeRange;

    [Header("Ranged Attack")]
    [SerializeField] private int numProjectile;
    private int projectileCount = 0;
    [SerializeField] private float spreadAngle;

    [Header("Special Attack (Dash)")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    private Vector2 dashDirection;
    private bool hasDealtDamage = true;

    [Header("Wander Area")]
    [SerializeField] private float wanderWidth = 30;
    [SerializeField] private float wanderHeight = 15;
    [SerializeField] private Vector2 startingPosition;
    [SerializeField] private float pausedDuration = 1.5f;

    private bool isPaused = false;
    private bool isAttacking = false;

    private float attackCooldownTimer;
    private Vector2 target;
    public EnemyState currentState;
    public enum EnemyState
    {
        Idle,
        Wander,
        MeleeAttack,
        RangedAttack,
        SpecialAttack,
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        target = GetRandomTarget();
    }

    private void Update()
    {
        attackCooldownTimer -= Time.deltaTime;
        SetState();
    }

    private void SetState()
    {
        if (isAttacking)
        {
            if (currentState == EnemyState.MeleeAttack)
            {
                MeleeAttack(); // keep re-evaluating chase-vs-attack every frame
            }
            return;
        }

        if (attackCooldownTimer <= 0)
        {
            ChangeState(PickAttackState());
        }
        else
        {
            ChangeState(EnemyState.Wander);
        }
    }

    private EnemyState PickAttackState()
    {
        //Pick new wander destination
        target = GetRandomTarget();

        //Pick between the three attack types 
        int attack = Random.Range(0, 3);
        return attack switch
        {
            0 => EnemyState.MeleeAttack,
            1 => EnemyState.RangedAttack,
            _ => EnemyState.SpecialAttack,
        };
    }

    private void ChangeState(EnemyState state)
    {
        currentState = state;

        switch (state)
        {
            case EnemyState.Idle:
                isPaused = true;
                anim.Play("Idle");
                break;

            case EnemyState.Wander:
                Wander();
                break;

            case EnemyState.MeleeAttack:
                StopAllCoroutines();
                isPaused = false;
                MeleeAttack();
                break;

            case EnemyState.RangedAttack:
                StopAllCoroutines();
                isPaused = false;
                RangedAttack();
                break;

            case EnemyState.SpecialAttack:
                StopAllCoroutines();
                isPaused = false;
                SpecialAttack();
                break;
        }
    }

    #region Melee Attack
    private void MeleeAttack()
    {
        isAttacking = true;
        if (rb.bodyType == RigidbodyType2D.Kinematic) return;

        if (Vector2.Distance(transform.position, player.position) < meleeRange / 1.5)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            anim.Play("Melee Attack");
        }
        else
        {
            //Not in range yet — keep chasing
            if (player.position.x > transform.position.x && transform.localScale.x < 0 ||
                player.position.x < transform.position.x && transform.localScale.x > 0)
            {
                Flip();
            }
            anim.Play("Walk");
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed * 2;
        }
    }

    //Trigger method that got call from animation
    public void FinishMeleeAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, meleeRange, playerLayer);
        if (hits.Length > 0)
        {
            hits[0].GetComponent<Player_HP>().ChangeHP(enemySO.enemyDamage);
        }

        rb.bodyType = RigidbodyType2D.Dynamic;
        isAttacking = false;
        attackCooldownTimer = attackCooldown;
    }
    #endregion

    #region Ranged Attack
    private void RangedAttack()
    {
        if (projectileCount < numProjectile)
        {
            if (player.position.x > transform.position.x && transform.localScale.x < 0 ||
                player.position.x < transform.position.x && transform.localScale.x > 0)
            {
                Flip();
            }

            isAttacking = true;
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            anim.Play("Ranged Attack", 0, 0f);
        }
        else
        {
            FinishRangedAttack();
        }
    }

    //Trigger method that got call from animation
    public void FireProjectile()
    {
        Vector2 dir = (player.position - attackPoint.position).normalized;

        float randomOffset = Random.Range(-spreadAngle, spreadAngle);
        dir = Quaternion.Euler(0f, 0f, randomOffset) * dir;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Enemy_Projectile projectile = Instantiate(enemyProjectilePrefab, attackPoint.position, Quaternion.Euler(0f, 0f, angle)).GetComponent<Enemy_Projectile>();
        projectile.direction = dir;
        projectile.sr.sprite = projectileSprite;
        projectile.enemySO = enemySO;
        projectile.transform.localScale = new Vector3(3f, 3f, 1f);

        projectileCount++;
        RangedAttack();
    }

    public void FinishRangedAttack()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        isAttacking = false;
        attackCooldownTimer = attackCooldown;

        projectileCount = 0;
    }
    #endregion

    #region Special Attack (Dash)
    private void SpecialAttack()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero;
        anim.Play("Charge Attack");
    }

    // Call this from an Animation Event at the end of the "Charging" clip
    public void LaunchDash()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        hasDealtDamage = false;
        StartCoroutine(Dash());
    }
    private IEnumerator Dash()
    {
        dashDirection = (player.position - transform.position).normalized; //Lock direction
        float angle = Mathf.Atan2(dashDirection.y, dashDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

        float timer = 0f;
        while (timer < dashDuration)
        {
            rb.velocity = dashDirection * dashSpeed; // reassert every physics step, overrides any push
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.velocity = Vector2.zero;
        transform.rotation = Quaternion.identity; // reset rotation back to normal
        attackCooldownTimer = attackCooldown;
        isAttacking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAttacking || hasDealtDamage) return;

        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            collision.gameObject.GetComponent<Player_HP>().ChangeHP(enemySO.enemyDamage);
            Debug.Log($"Dealt {enemySO.enemyDamage} damage to {collision}");
        }
        hasDealtDamage = true;
    }
    #endregion

    #region Wander methods
    private void Wander()
    {
        if (isPaused)
        {
            anim.Play("Idle");
            rb.velocity = Vector2.zero;
            return;
        }

        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            StartCoroutine(PauseAndPickNewDestination());
            return;
        }

        Vector2 direction = (target - (Vector2)transform.position).normalized;

        if (target.x > transform.position.x && transform.localScale.x < 0 ||
            target.x < transform.position.x && transform.localScale.x > 0)
        {
            Flip();
        }

        anim.Play("Walk");
        rb.velocity = direction * speed;
    }

    private IEnumerator PauseAndPickNewDestination()
    {
        isPaused = true;
        anim.Play("Idle");

        yield return new WaitForSeconds(pausedDuration);

        target = GetRandomTarget();
        isPaused = false;
        anim.Play("Walk");
    }

    void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private Vector2 GetRandomTarget()
    {
        float halfWidth = wanderWidth / 2;
        float halfHeight = wanderHeight / 2;
        int edge = Random.Range(0, 4);

        return edge switch
        {
            0 => new Vector2(startingPosition.x - halfWidth, Random.Range(startingPosition.y - halfHeight, startingPosition.y + halfHeight)), //Left
            1 => new Vector2(startingPosition.x + halfWidth, Random.Range(startingPosition.y - halfHeight, startingPosition.y + halfHeight)), //Right
            2 => new Vector2(Random.Range(startingPosition.x - halfWidth, startingPosition.x + halfWidth), startingPosition.y - halfHeight), //Bottom
            _ => new Vector2(Random.Range(startingPosition.x - halfWidth, startingPosition.x + halfWidth), startingPosition.y + halfHeight), //Top
        };
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, 1);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(startingPosition, new Vector3(wanderWidth, wanderHeight, 0));
    }
}