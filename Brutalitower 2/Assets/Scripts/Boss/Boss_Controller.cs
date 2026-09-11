using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Boss_Controller : MonoBehaviour
{
    //Stat variables
    public Set_Boss_Stats base_stats;
    private float max_health;
    public float current_health;

    //components
    public Animator animator;

    //Phase management
    [SerializeField] private Transform boss_spawn_location;
    [SerializeField] private GameObject Boss_Phase_Three;
    public int current_Phase;

    [SerializeField] private ParticleSystem Blood;  // Particle system for blood spatter

    //Ui Elementse
    private Image currenthalthBar;
    private float time;
    private float shown_health;

    [SerializeField] Image[] health_fill;
    [SerializeField] Image[] health_frame;

    // Start is called before the first frame update
    void Start()
    { 
        //Set components
        animator = GetComponent<Animator>();
        base_stats = GetComponent<Set_Boss_Stats>();
        set_Current_Stats();
        activeHealthBar();
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() //Functions that need less priority
    {
        BossHealthBar();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Damage") && Time.time != time)
        {
            time = Time.time;
            Take_damage(Game_Manager.instance.Current_Stats.Damage);
        }
    }

    void activeHealthBar()
    {
        for(int i = 0; i < health_fill.Length; i++) //Set current health bar
        {
            if(i == current_Phase) //the health bar should match the phase
            {
                health_fill[i].enabled = true;
                currenthalthBar = health_fill[i];
                health_frame[i].enabled = true;
            }
            else
            {
                health_fill[i].enabled = false;
                health_frame[i].enabled = false;
            }
        }
    }

    public void Take_damage(float damage) //refrence in collider that takes damage
    {
        current_health -= damage; //apply damage
        Blood.Play(); // Plays the bone particles
        if (current_health <= 0)
        {
            if(Game_Manager.instance.Current_Stats.Current_loop >= 1) //if the player looped once activate phase_two 
            {

                if(current_Phase == Game_Manager.instance.Current_Stats.Current_loop)
                {
                    animator.SetBool("Dead", true); //if in phase two die when health reaches zero
                }
                else
                {                  
                    Next_Phase();                 
                }
                activeHealthBar();

            }
            else
            {
                animator.SetBool("Dead", true); //kill boss if is the first one
            }
        }
    }

    private void Next_Phase()
    {
        max_health += (base_stats.Health/2);
        current_health = max_health; //refresh health to max
        current_Phase += 1; //activate boss phase two
    }

    void set_Current_Stats()
    {
        max_health = base_stats.Health;
        current_health = base_stats.Health; //Set health
    }

    void death()
    {
        Game_Manager.instance.Current_Stats.Current_loop += 1;
        if(current_Phase == 2)
        {
            Game_Manager.instance.leave_Scene("Credits");
        }
        else
        {
            Game_Manager.instance.leave_Scene("Game_Area");
        }
        Destroy(gameObject);
    }
    public void BossHealthBar() //Show Healthbar
    {
        currenthalthBar.fillAmount = Mathf.Lerp(shown_health, current_health / max_health, .2f);
        shown_health = currenthalthBar.fillAmount;
    }

    public void Flip_boss_Direction(int direction)
    {
        Vector3 localScale = transform.localScale;
        localScale.x = direction;
        transform.localScale = localScale;
    }
    public void Flip_To_Player()
    {
        Vector2 PlayerLoc = Game_Manager.instance.player.transform.position - transform.position;

        if (PlayerLoc.x < 0)
        {
            Vector3 flipScale = transform.localScale;
            flipScale.x = -1;
            transform.localScale = flipScale;
        }
        else
        {
            Vector3 flipScale = transform.localScale;
            flipScale.x = 1;
            transform.localScale = flipScale;
        }
    }
}
