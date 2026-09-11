using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player_Controller : MonoBehaviour
{

    private CinemachineImpulseSource impulseSource;
    [SerializeField] private ParticleSystem _particleSystem;
    private Animator _animator;
    private Player_Attack player_Attack;
    private Rigidbody2D rb;
    
    public bool canTakeDamage;



    private void Start()
    {
        canTakeDamage = true;
        Physics2D.IgnoreLayerCollision(3,8); //Ignore enemies walk through em
        Get_Components(); //Gets componenets needed


    }

    private void Get_Components()
    {
        _animator = GetComponent<Animator>();
        impulseSource = GetComponent<CinemachineImpulseSource>(); //Gets Impulse Source
        player_Attack = GetComponent<Player_Attack>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        
    }

    private void Death()
    {
        if(Game_Manager.instance.Current_Stats.Current_Health <= 0f) //if player is out of health its dead
        {
            _animator.SetBool("Dead", true);
            StartCoroutine(Restart_Level());
        }
    }

    private IEnumerator Restart_Level()
    {
        yield return new WaitForSeconds(2f);
        Game_Manager.instance.leave_Scene("Game_Area");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("E_Damage") && canTakeDamage == true && _animator.GetBool("Dead") == false)
        {
            take_Damage(2f);
        }
        if (collision.CompareTag("BishopDamage") && canTakeDamage == true && _animator.GetBool("Dead") == false)
        {
            take_Damage(1f);
        }
    }


    void take_Damage(float damage)
    {
        SoundManager.PlaySound(SoundType.PlayerHit, .7f);
        _animator.SetTrigger("Damaged"); //hit animation
        canTakeDamage = false;
        _particleSystem.Play(); //blood splatter
        StartCoroutine(Slow_Time()); //slow time for a brief second
        //apply damage
        Game_Manager.instance.Current_Stats.Current_Health -= damage; // apply five damage
        
        Game_Manager.instance.Camera_Shake(impulseSource); //Shake Camera
        player_Attack.End_attack(); //Ends attack to prevent bugs
        Death();
    }

    private IEnumerator Slow_Time()
    {
        Time.timeScale = .6f;
        yield return new WaitForSeconds(.3f);
        Time.timeScale = 1;
        yield return new WaitForSeconds(.5f);
        canTakeDamage = true;
    }

}
