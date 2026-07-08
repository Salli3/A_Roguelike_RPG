using System.Collections;
using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Animator anim;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Player_Controler playerControler;
    private void Awake()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    public void Attack()
    {
        //TODO make weapon attack anim name holder in class
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
        playerControler.ResetAttackTimer();
    }

    private Vector2 GetAimDirection()
    {
        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        return (mouseWorld - (Vector2)attackPoint.position).normalized;
    }
}