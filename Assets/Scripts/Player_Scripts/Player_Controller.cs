using UnityEngine;

public class Player_Controler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private Player_Combat playerCombat;

    private float timer;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                playerCombat.Attack();
            }
        }
    }

    public void ResetAttackTimer()
    {
        timer = Stats_Manager.instance.cooldown;
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        anim.SetFloat("horizontal", Mathf.Abs(horizontal));
        anim.SetFloat("vertical", Mathf.Abs(vertical));

        rb.velocity = new Vector2(horizontal, vertical).normalized * Stats_Manager.instance.speed;
    }
}
