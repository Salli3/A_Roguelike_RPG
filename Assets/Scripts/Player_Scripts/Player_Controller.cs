using UnityEngine;

public class Player_Controler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private Player_Combat playerCombat;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerCombat.Attack();
            Debug.Log("Gotten in put");
        }
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
