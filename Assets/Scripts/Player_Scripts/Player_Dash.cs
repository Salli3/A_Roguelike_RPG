using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Dash : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private Player_Controler playerControler;

    public void TriggerDash(Vector2 direction)
    {
        StartCoroutine(Dash(direction));
    }

    private IEnumerator Dash(Vector2 direction)
    {
        rb.velocity = direction * Stats_Manager.instance.dashSpeed;
        anim.speed = 1 / Stats_Manager.instance.dashDuration;
        anim.Play("Dash");

        yield return new WaitForSeconds(Stats_Manager.instance.dashDuration);
        anim.speed = 1f;
        rb.velocity = Vector2.zero;
        playerControler.ResetDashTimer();
    }
}