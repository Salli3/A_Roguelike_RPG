using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Boss_Behaviour;

public class Enemy_Knockback : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    public bool isKnockedback;

    public void Knockback(Transform forceTransform)
    {
        Vector2 direction = (transform.position - forceTransform.position).normalized;
        rb.velocity = direction * Stats_Manager.instance.knockbackForce;
        isKnockedback = true;
        StartCoroutine(KnockbackCounter());
    }
    IEnumerator KnockbackCounter()
    {
        yield return new WaitForSeconds(Stats_Manager.instance.knockbackTime);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(Stats_Manager.instance.stunTime);
        isKnockedback = false;
    }
}
