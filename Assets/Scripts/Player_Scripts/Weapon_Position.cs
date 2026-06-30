using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Weapon_Position : MonoBehaviour
{
    [SerializeField] private Transform weaponTransform;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Camera mainCamera;

    private int facingDirection = 1;

    private void Awake()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    private void Update()
    {
        UpdateWeaponPosition();
    }

    private void UpdateWeaponPosition()
    {
        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mouseWorld - (Vector2)playerTransform.position).normalized;

        weaponTransform.position = playerTransform.position + (Vector3)(dir * Stats_Manager.instance.attackRange);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        weaponTransform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (playerTransform.position.x - weaponTransform.position.x < 0 && transform.localScale.x < 0 ||
                playerTransform.position.x - weaponTransform.position.x > 0 && transform.localScale.x > 0)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        weaponTransform.localScale = new Vector3(weaponTransform.localScale.x * -1 , weaponTransform.localScale.y * -1, weaponTransform.localScale.z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerTransform.position, 1);
    }
}
