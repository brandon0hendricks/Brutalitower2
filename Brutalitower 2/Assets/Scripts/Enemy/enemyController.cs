using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    // This is the brain for the Enemy combattants!

    public enum EnemyTypes // The names of all enemy types
    {
        Charger,
        Bishop,
    };
    public EnemyTypes enemyTypes;                           // An instance of the previous enum
    [HideInInspector] public enemyStats chosenStats;        // This is set to the stats of whichever enemy is chosen
    [HideInInspector] public Animator anim;                 // This is the animator. Also chosen through the stats.
    private CapsuleCollider2D collider_;
    public GameObject wallChecker;
    public GameObject groundChecker; 
    [HideInInspector] public GameObject playerLocation;     // This is the player
    private enemyMovement_ movementScript;                  // Reference to the movement script
    [SerializeField] private ParticleSystem boneParticles;  // Particle system for bone spatter
    [SerializeField] private ParticleSystem healParticles;  // Healing particles
    private float health;                                   // The health of the enemy, set through the stats
    private float maxHealth;
    public bool exploration = false;
    [SerializeField] private Transform healthBar;
    private float time;
    public Action<GameObject> death;

    private void Awake()    // Grabs the animator, the movement script, and player
    {
        anim = gameObject.GetComponent<Animator>();
        collider_ = gameObject.GetComponent<CapsuleCollider2D>();
        movementScript = gameObject.GetComponent<enemyMovement_>();
        playerLocation = GameObject.FindWithTag("Player");
        enabled = false;
    }
    public void LateStart() // Sets all the stats, is ran by the spawner script during spawning
    {
        anim.runtimeAnimatorController = chosenStats.animatorController;
        movementScript.speed = chosenStats.speed;
        movementScript.jumpPower = chosenStats.jumpHeight;
        movementScript.attackCooldown = chosenStats.cooldown;
        health = chosenStats.health;
        maxHealth = health;
        if (enemyTypes == EnemyTypes.Bishop)
        {
            collider_.offset = new Vector2(collider_.offset.x, -0.61f);
            collider_.size = new Vector2(collider_.size.x, 1.61f);
            wallChecker.transform.localPosition = new Vector2(wallChecker.transform.localPosition.x, -1f);
            groundChecker.transform.localPosition = new Vector2(groundChecker.transform.localPosition.x, -1.338f);
        }

        enabled = true;
        Physics2D.IgnoreLayerCollision(8, 8); //Ignore enemies walk through em
    }

    public void HealthBar()
    {
        float target = health / maxHealth;
        Vector3 nextLocation = healthBar.transform.localScale;
        nextLocation.x = Mathf.Lerp(nextLocation.x, target, 0.2f);
        healthBar.transform.localScale = nextLocation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Damage") && Time.time != time)
        {
            time = Time.time;
            TakeDamage(Game_Manager.instance.Current_Stats.Damage);
        }
        if (collision.CompareTag("BishopDamage") && Time.time != time)
        {
            time = Time.time;
            Heal(1f);
        }
    }

    void TakeDamage(float damage) // Activates when hit by the player
    {
        health -= damage;
        SoundManager.PlaySound(SoundType.SkeletonDamage, 1f);
        boneParticles.Play(); // Plays the bone particles
        if (health <= 0f)
        {
            movementScript.rb.linearVelocity = Vector3.zero;
            anim.SetBool("Death", true); // If enemy is out of health, remove it
            movementScript.disabled = true;
        }
    }

    void Heal(float healAmount)
    {
        health += healAmount;
        if (health > maxHealth)
        {
            health = maxHealth; // Prevents overhealing
        }
        healParticles.Play();
    }

    void Update()
    {
        if (Game_Manager.instance.Current_Stats.Current_Health <= 0f)
        {
            movementScript.disabled = true; // If player dies, enemy stops moving
        }
        HealthBar();   
    }

    void Death()
    {
        int currencyDecision = UnityEngine.Random.Range(0, 2);
        Game_Manager.instance.Current_Stats.Current_Health += 1f;
        if (currencyDecision == 0)
        {
            Game_Manager.instance.Current_Stats.Goldcoins += UnityEngine.Random.Range(9, 16);
        }
        else
        {
            Game_Manager.instance.Current_Stats.Bonedust += UnityEngine.Random.Range(2, 5);
        }
        death?.Invoke(gameObject);
        Destroy(gameObject); //Play in animator
    }
}


