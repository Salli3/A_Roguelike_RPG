using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Rigidbody2D rb;
    public Vector2 direction = Vector2.right;
    
    [SerializeField] private float speed;

    [SerializeField] private LayerMask enemyLayer;

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void Start()
    {
        sr.sprite = Stats_Manager.instance.projectileSprite;
        rb.velocity = direction * speed;
        RotateProjectile();
        Destroy(gameObject, Stats_Manager.instance.attackRange);
    }

    private void RotateProjectile()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Hit: " + collision.gameObject.name + " | Layer: " + LayerMask.LayerToName(collision.gameObject.layer));
        if ((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            //Debug.Log($"Dealt {Stats_Manager.instance.damage} to {collision.gameObject}");
            collision.gameObject.GetComponent<Enemy_HP>().ChangeHP(Stats_Manager.instance.damage);
            AttachToTarget(collision.gameObject.transform);
        }       
    }

    private void AttachToTarget(Transform target)
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;

        transform.SetParent(target);
    }
}
