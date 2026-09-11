using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    //player Input
    private float horizontal;
    private bool can_Move = true;

    //Direction of player
    private bool isFacingRight = true;

    //Player Components
    private Rigidbody2D rb;
    public Animator animator;
    private Player_Stats stats;
    private Player_Controller controller;
    float dodgeRecoverTime;

    //Player Ground Check position
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        stats = Game_Manager.instance.Current_Stats;
        Set_Components();
        
    }
    // Update is called once per frame
    void Update()
    {

        if(animator.GetBool("Dead") == false)
        {
            InputHiarchy(); //runs input system
        }
        else
        {
            if(IsGrounded())
            {
                rb.velocity = Vector3.zero; //if dead stop movement
                
            }
        }         
    }

    void InputHiarchy()
    {
        Recover_Stam();
        Slide();
        PlayerInput();
        if (animator.GetBool("InAttack") == false)
        {
            Jump();
            if (can_Move == true)
            {
                Movement();
                UpdateFlip();
            }

        }
        else if(animator.GetBool("InAttack") == true)
        {
            rb.velocity = new Vector2(horizontal / (stats.Speed / 2), rb.velocity.y);
        }
    }


    void Set_Components()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<Player_Controller>();
    }

    void PlayerInput()
    {

        //Get player input
        horizontal = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", rb.velocity.y);
        animator.SetBool("IsGrounded", IsGrounded());
    }
    void Movement()
    {
       
            rb.velocity = new Vector2(horizontal * stats.Speed, rb.velocity.y); //Horizontal movement
    }

    //checks if player is grounded
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    void Jump()
    {
        //Jump Mechanic
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, stats.JumpPower);
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            if (rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .5f);
            }
        }

        fast_Fall();
    }

    void Slide()
    {   
        
        Player_Attack player_Attack = GetComponent<Player_Attack>();
        if(Input.GetKeyDown(KeyCode.LeftShift) && stats.Dodges > 0)
        {
            SoundManager.PlaySound(SoundType.Slide, 8f);
            stats.Dodges -= 1;
            animator.SetBool("Sliding", true); //starts slide
            if (animator.GetBool("InAttack") == true)
            {
                player_Attack.End_attack();
            }
            can_Move = false;
            controller.canTakeDamage = false;
            rb.velocity = new Vector2(transform.localScale.x * 9.5f, rb.velocity.y);
            //make Immune
        }
    }

    void end_Slide()
    {
        animator.SetBool("Sliding", false);
        can_Move = true;
        rb.velocity = Vector2.zero;      
        controller.canTakeDamage = true;
        //turn off imunity
    }

    void fast_Fall()
    {
        float fastFall = 1.7f;
        if (!IsGrounded() && rb.velocity.y < 0f)
        {
            //Debug.Log("Fast Fall activated");
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fastFall) * Time.deltaTime;
        }
    }

    void Recover_Stam()
    {
        dodgeRecoverTime += Time.deltaTime;
        if(dodgeRecoverTime >= 3f && stats.Dodges < stats.Max_Dodges)
        {
            dodgeRecoverTime = 0;
            stats.Dodges += 1;
        }
    }

    private void UpdateFlip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    

}
