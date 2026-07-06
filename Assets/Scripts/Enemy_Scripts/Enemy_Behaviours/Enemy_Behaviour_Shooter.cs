using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy_Behaviour_Shooter : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float runAwayRange;
    [SerializeField] private float attackCooldown;

    private float attackCooldownTimer;

    [SerializeField] private Enemy_SO enemySO;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform player;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject enemyProjectilePrefab;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Sprite projectileSprite;

    [Header("Wander Area")]
    public float wanderWidth = 5;
    public float wanderHeight = 5;
    public Vector2 startingPosition;
    public float pausedDuration = 1.5f;

    private bool isPaused = false;
    private bool isShooting = false;

    private Vector2 target;

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
        if (isShooting)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (attackCooldownTimer <= 0)
        {
            Shoot();
        }
        else if (Vector2.Distance(transform.position, player.position) < runAwayRange)
        {
            isPaused = false;
            StopAllCoroutines();
            target = GetRandomTarget();
            Run();
        }
        else
        {
            if (isPaused)
            {
                anim.Play("Idle");
                rb.velocity = Vector2.zero;
                return;
            }
            if (Vector2.Distance(transform.position, target) < 0.1f && isPaused == false)
            {
                StartCoroutine(PauseAndPickNewDestination());
            }
            Move();
        }
    }

    #region Attack Methods
    private void Shoot()
    {
        rb.velocity = Vector2.zero;

        if (player.position.x > transform.position.x && transform.localScale.x < 0 ||
            player.position.x < transform.position.x && transform.localScale.x > 0)
        {
            Flip();
        }

        anim.Play("Shooting");
        isShooting = true;
    }

    public void FinishShooting()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Enemy_Projectile projectile = Instantiate(enemyProjectilePrefab, attackPoint.position, Quaternion.Euler(0f, 0f, angle)).GetComponent<Enemy_Projectile>();
        projectile.direction = dir;
        projectile.sr.sprite = projectileSprite;
        projectile.enemySO = enemySO;

        isShooting = false;
        attackCooldownTimer = attackCooldown;
    }
    #endregion

    private void Run()
    {
        if (player.position.x > transform.position.x && transform.localScale.x > 0 ||
            player.position.x < transform.position.x && transform.localScale.x < 0)
        {
            Flip();
        }
        anim.Play("Chase");
        Vector2 direction = - (player.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    #region Wander Methods
    private void Move()
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;

        if (direction.x > 0 && transform.localScale.x < 0 ||
            direction.x < 0 && transform.localScale.x > 0)
        {
            Flip();
        }

        anim.Play("Chase");
        rb.velocity = direction * speed;
    }

    void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    IEnumerator PauseAndPickNewDestination()
    {
        isPaused = true;
        anim.Play("Idle");

        yield return new WaitForSeconds(pausedDuration);

        target = GetRandomTarget();
        isPaused = false;
        anim.Play("Chase");
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
        Gizmos.DrawWireSphere(transform.position, runAwayRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(startingPosition, new Vector3(wanderWidth, wanderHeight, 0));
    }
}