using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask playerLayer;

    public SpriteRenderer sr; 
    public Enemy_SO enemySO;
    public Vector2 direction = Vector2.right;

    private void Start()
    {
        rb.velocity = direction * speed;
        Destroy(gameObject, 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Hit: " + collision.gameObject.name + " | Layer: " + LayerMask.LayerToName(collision.gameObject.layer));
        if ((playerLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            //Debug.Log($"Dealt {Stats_Manager.instance.damage} to {collision.gameObject}");
            collision.gameObject.GetComponent<Player_HP>().ChangeHP(enemySO.enemyDamage);
            Destroy(gameObject);
        }
    }
}
