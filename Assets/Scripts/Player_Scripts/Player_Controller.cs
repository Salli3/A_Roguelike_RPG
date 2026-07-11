using UnityEngine;

public class Player_Controler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private Player_Combat playerCombat;
    [SerializeField] private Player_Dash playerDash;

    public float timer = 0f;

    public float dashCooldownTimer = 0f;
    public bool isDashing = false;
    private Vector2 currentDirection = Vector2.down;

    private void Update()
    {
        timer -= Time.deltaTime * Stats_Manager.instance.attackSpeed;
        if (timer < 0)
        {
            if (Input.GetMouseButton(0))
            {
               
                playerCombat.Attack();
            }
        }

        dashCooldownTimer -= Time.deltaTime;
        if (dashCooldownTimer < 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
            {
                isDashing = true;
                playerDash.TriggerDash(currentDirection);
            }
        }
    }

    public void ResetAttackTimer()
    {

        timer = 1f;
    }

    public void ResetDashTimer()
    { 
        dashCooldownTimer = Stats_Manager.instance.dashCooldown;
        isDashing = false;
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        currentDirection = new Vector2(horizontal, vertical).normalized;

        anim.SetFloat("horizontal", Mathf.Abs(horizontal));
        anim.SetFloat("vertical", Mathf.Abs(vertical));

        rb.velocity = new Vector2(horizontal, vertical).normalized * Stats_Manager.instance.speed;
    }
}
