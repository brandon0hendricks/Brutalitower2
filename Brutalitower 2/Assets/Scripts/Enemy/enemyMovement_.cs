using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemyMovement_ : MonoBehaviour
{
    // This script is used by the enemy controller to move the enemy around!

    public Rigidbody2D rb;                                 // Just the RigidBody2D component
    private GameObject player;                              // The player reference
    private enemyController enemyControllerScript;          // This is a reference to the enemyControllerScript script
    private Vector2 enemyLocation;                          // This is the location of the enemy
    [HideInInspector] public float jumpPower;               // How strong can the enemy jump? This is set from the stats SO
    [HideInInspector] public float speed;                   // How fast is the enemy? Also set from the stats SO
    [SerializeField] private Transform wallCheck;           // The location of the object used to check for walls
    [SerializeField] private LayerMask wallLayer;           // This indicates which layer is the walls
    [SerializeField] private Transform groundCheck;         // The location of the object used to check for ground
    [SerializeField] private LayerMask groundLayer;         // This indicates which layer is the ground
    [SerializeField] private float attackDashSpeed;         // This is how fast the enemy moves during the attack
    [HideInInspector] public float attackCooldown;          // This is how long it waits before it can attack again. Also set by Stats SO
    [SerializeField] private GameObject hitbox;             // This is the hitbox that attacks the player
    public float heightDistance;                            // This controls the height difference from the player to the enemy to tell the enemy if it should continue pursuing the player
    private Coroutine runningAttack;                        // This is a reference to the attack coroutine, is used as a sort of timer.
    [HideInInspector] public bool disabled;                 // This is used to disable the enemy movement, used when the player dies
    [SerializeField] private Transform playerDetector;
    [SerializeField] private float playerDetectionRadius;
    [SerializeField] private LayerMask playerLayer;         // This is used to detect the player
    private bool playerWasSeen;
    [SerializeField] private GameObject bishopsBalls;       // Gameobject prefab for the Bishop's attack
    [SerializeField] private Transform bishopBallLocation;

    void Awake() // Assigns the RB, assigns the enemy controller script, and grabs the player location from the controller script
    {
        rb = GetComponent<Rigidbody2D>();
        enemyControllerScript = GetComponent<enemyController>();
    }

    void Start()
    {
        player = enemyControllerScript.playerLocation;
    }

    void Update()
    {
        enemyLocation = player.transform.position - transform.position; // Finds the difference of the player and enemy locations and uses that for movement
        if (!enemyControllerScript.anim.GetBool("Attacking") && !disabled && !enemyControllerScript.exploration)   // If we aren't attacking, move! And check to see if we can attack.
        {
            if (Mathf.Abs(enemyLocation.x) > enemyControllerScript.chosenStats.chaseDistance || enemyLocation.y >= heightDistance)  // If in chase mode, RUN
            {
                Move();
            }   //Look, Brandon! I added brackets! Hooray!
        }
        if (Mathf.Abs(enemyLocation.x) <= enemyControllerScript.chosenStats.chaseDistance && enemyLocation.y < heightDistance && runningAttack == null) // If within the chase distance, go idle
        {
            runningAttack = StartCoroutine(RunAttack());
        }
        if (Mathf.Abs(rb.linearVelocity.x) > 0.005f)  // If we are moving, look like it.
        {
            enemyControllerScript.anim.SetBool("Walking", true);
        }
        else if (Mathf.Abs(rb.linearVelocity.x) <= 0.005f)    // If we aren't moving, don't look like you are
        {
            enemyControllerScript.anim.SetBool("Walking", false);
        }

        if (enemyControllerScript.exploration)
        {
            if (LineOfSight() || playerWasSeen)
            {
                ExploreChase();
            }
            else
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }
    }

    private void Move()     // This tells the enemy where to go, and when to jump
    {
        rb.linearVelocity = new Vector2(Mathf.Clamp(enemyLocation.x, -1, 1) * speed, rb.linearVelocity.y);  // Set velocity towards the player, at the set speed

        if (IsWalled() && IsGrounded())   // If hitting a wall, jump
        {
            Jump();
        }
        Flip();     // Like flipsie from fairly odd parents
    }

    private void ExploreChase()
    {
        if (PlayerDetection())
        {
            playerWasSeen = true;
            Move();
        }
        else
        {
            playerWasSeen = false;
        }
    }

    private void Jump()     // Dead simple, same as horizontal movement, just vertical. GET THAT DUNK
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
    }

    private void Attack()
    {
        rb.linearVelocity = new Vector2(transform.localScale.x * (speed * attackDashSpeed), 0);
    }

    private void Flip()     // This is how the enemy flips outside of attack. 
    {
        if (rb.linearVelocity.x < 0)
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

    private void FlipToPlayer()     // This is how the enemy flips when entering an attack
    {
        if (enemyLocation.x < 0)
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

    private bool IsWalled() // This is the wall checker
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.1f, wallLayer);
    }

    private bool IsGrounded() // This is the ground check
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool PlayerDetection()
    {
        return Physics2D.OverlapCircle(playerDetector.position, playerDetectionRadius, playerLayer); 
    }
    private bool LineOfSight()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerDetector.position, player.transform.position - playerDetector.position, playerDetectionRadius, playerLayer);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }

    private IEnumerator RunAttack() // This is how the attack is ran
    {
        rb.linearVelocity = Vector2.zero;
        FlipToPlayer();
        enemyControllerScript.anim.SetBool("Attacking", true);
        yield return new WaitForSeconds(attackCooldown);
        runningAttack = null;
    }

    public void AttackEnd() // This is ran at the end of the animator.
    {
        enemyControllerScript.anim.SetBool("Attacking", false);
    }
    public void ChargerHitboxActivate() // This activates the hitbox to hit the player, ran in animator
    {
        SoundManager.PlaySound(SoundType.ChargerMelee, .7f);
        hitbox.SetActive(true);
        Attack();
    }
    public void ChargerHitboxDeactivate() // This just undoes the previous.
    {
        hitbox.SetActive(false);
        rb.linearVelocity = new Vector2(0, 0);
    }
    public void BishopAttack()
    {
        SoundManager.PlaySound(SoundType.BishopMagic, .7f);
        Instantiate(bishopsBalls, bishopBallLocation.position, Quaternion.identity);
    }
}
