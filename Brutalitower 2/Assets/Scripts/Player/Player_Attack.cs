using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_Attack : MonoBehaviour
{

    private float CT_Attack; //Timer for how much time has passed since last attack
    private int current_hit;
    private Player_Movement Player_Movement;

    private int Current_Hit
    {
        get { return current_hit; }
        set
        {
            if (value >= 3) //if current hit exceeds maxium combos restart combo
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                current_hit = 1;
            }
            else
            {
                current_hit = value;
            }
        }
    }
    public Animator animator;
    [SerializeField] private GameObject Hit_box;
    //Player Components
    private Rigidbody2D rb;

    //variables for air attack
    private bool will_fall;

    private void Start()
    {
        Components();

    }

    void Components()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        Player_Movement = gameObject.GetComponent<Player_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack_Input();

        CT_Attack += Time.deltaTime; //tracks time passed
        animator.SetInteger("Current_Hit", current_hit); //Set What attack is happening

        if(Player_Movement.IsGrounded())
        {
            will_fall = true;
        }
    }
    void Attack_Input()
    {
        if (Input.GetMouseButtonDown(0)) //start attack only if not in a attack currently 
        {
            if (animator.GetBool("InAttack") == false && animator.GetBool("Sliding") == false && gameObject.GetComponent<Player_Controller>().canTakeDamage == true)
            {
               
                if (will_fall == true)
                {
                    will_fall = false;
                    rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                }
                Current_Hit = 1;
                animator.SetTrigger("Attack");
                animator.SetBool("InAttack", true);
            }
            CT_Attack = 0;
        }
    }
    void Momentum()//add to frame to push player slightly forward
    {
        rb.AddForce(new Vector2(transform.localScale.x * 15f,rb.linearVelocity.y)*20);
        Game_Manager.instance.Camera_Shake(gameObject.GetComponent<CinemachineImpulseSource>());
    }

    void Activate_damage() //This turns on damage during the attack frame 
    {
        SoundManager.PlaySound(SoundType.PlayerDamage, .7f);
        Hit_box.SetActive(true);
    }
    void Next_Hit()//checks if player attacked within the last few frames and attack as soon as available if so
    {
        Hit_box.SetActive(false);
        if (animator.GetBool("InAttack") == true)
        {
            if (CT_Attack <= .18f && animator.GetBool("Sliding") == false) // do next attack if recently input
            {
                animator.SetTrigger("Attack");
                Current_Hit += 1;
            }
            else //finnish attack if no input
            {
                Goto_Idle();
            }
        }
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    void Goto_Idle()
    {
        animator.SetBool("InAttack", false);
        animator.ResetTrigger("Attack");
        animator.SetTrigger("Finnished");
    }
    public void End_attack()
    {
        Hit_box.SetActive(false);
        animator.ResetTrigger("Attack");
        animator.SetBool("InAttack", false);
        animator.ResetTrigger("Finnished");
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
