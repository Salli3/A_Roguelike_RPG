using System.Collections;
using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Animator anim;
    [SerializeField] private Camera mainCamera;

    public LayerMask enemyLayer;

    private Vector2 aimDirection = Vector2.right;

    private void Awake()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    public void Attack()
    {
        anim.Play("Bow_Charge");
    }

    public void TriggerAttack()
    {
        if (Stats_Manager.instance.isRanged)
        {
            RangedAttack();
        }
        else
        {
            MeleeAttack();
        }
    }

    private void MeleeAttack()
    {
        //TODO make melee attack logic
    }

    private void RangedAttack()
    {
        Vector2 aimDirection = GetAimDirection();
        Projectile projectile = Instantiate(projectilePrefab, attackPoint.position, Quaternion.identity).GetComponent<Projectile>();
        projectile.Init(aimDirection);
        anim.Play("Idle");
    }

    private Vector2 GetAimDirection()
    {
        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        return (mouseWorld - (Vector2)attackPoint.position).normalized;
    }
}